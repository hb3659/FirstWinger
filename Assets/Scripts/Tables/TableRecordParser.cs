using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Text;

public class MarshalTableConstant
{
    public const int charBufferSize = 256;
}
public class TableRecordParser<TMarshalStruct>
{
    public TMarshalStruct ParserRecordLine(string _line)
    {
        // TMarshalStruct 크기에 맞춰서 Byte 배열 할당
        Type type = typeof(TMarshalStruct);
        int structSize = Marshal.SizeOf(type);
        byte[] structByte = new byte[structSize];
        int structByteIndex = 0;

        // _line 문자열을 splitter 로 자름
        const string splitter = ",";
        string[] fieldDataList = _line.Split(splitter.ToCharArray());
        // 타입을 보고 바이너리에 파싱하여 삽입
        Type dataType;
        string splited;
        byte[] fieldBytes;
        byte[] keyBytes;

        FieldInfo[] fieldInfos = type.GetFields();
        for (int index = 0; index < fieldInfos.Length; index++)
        {
            dataType = fieldInfos[index].FieldType;
            splited = fieldDataList[index];

            fieldBytes = new byte[4];
            MakeBytesByFieldType(out fieldBytes, dataType, splited);

            Buffer.BlockCopy(fieldBytes, 0, structByte, structByteIndex, fieldBytes.Length);
            structByteIndex += fieldBytes.Length;

            // 첫번째 필드르 key 값으로 사용하기 위해 백업
            if (index == 0)
                keyBytes = fieldBytes;
        }

        TMarshalStruct tStruct = MakeStructFromBytes<TMarshalStruct>(structByte);

        return tStruct;
    }

    protected void MakeBytesByFieldType(out byte[] _fieldByte, Type _dataType, string _split)
    {
        _fieldByte = new byte[1];

        if (typeof(int) == _dataType)
            _fieldByte = BitConverter.GetBytes(int.Parse(_split));
        else if (typeof(float) == _dataType)
            _fieldByte = BitConverter.GetBytes(float.Parse(_split));
        else if(typeof(bool) == _dataType)
        {
            bool value = bool.Parse(_split);
            int temp = value ? 0 : 1;

            _fieldByte = BitConverter.GetBytes((int)temp);
        }
        else if(typeof(string) == _dataType)
        {
            _fieldByte = new byte[MarshalTableConstant.charBufferSize];
            byte[] byteArr = Encoding.UTF8.GetBytes(_split);
            // 변환된 byte 배열을 고정 크기 버퍼에 복사
            Buffer.BlockCopy(byteArr, 0, _fieldByte, 0, byteArr.Length);
        }
    }

    public static T MakeStructFromBytes<T>(byte[] bytes)
    {
        int size = Marshal.SizeOf(typeof(T));
        IntPtr ptr = Marshal.AllocHGlobal(size);        // 마샬 메모리 할당

        Marshal.Copy(bytes, 0, ptr, size);              // 복사

        // 메모리로부터 T형 구조체로 변환
        T tStruct = (T)Marshal.PtrToStructure(ptr, typeof(T));
        Marshal.FreeHGlobal(ptr);       // 할당된 메모리 해제
        return tStruct;                 // 변환된 값 반환
    }
}
