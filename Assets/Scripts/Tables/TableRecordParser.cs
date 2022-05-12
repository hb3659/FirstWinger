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
        // TMarshalStruct ũ�⿡ ���缭 Byte �迭 �Ҵ�
        Type type = typeof(TMarshalStruct);
        int structSize = Marshal.SizeOf(type);
        byte[] structByte = new byte[structSize];
        int structByteIndex = 0;

        // _line ���ڿ��� splitter �� �ڸ�
        const string splitter = ",";
        string[] fieldDataList = _line.Split(splitter.ToCharArray());
        // Ÿ���� ���� ���̳ʸ��� �Ľ��Ͽ� ����
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

            // ù��° �ʵ帣 key ������ ����ϱ� ���� ���
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
            // ��ȯ�� byte �迭�� ���� ũ�� ���ۿ� ����
            Buffer.BlockCopy(byteArr, 0, _fieldByte, 0, byteArr.Length);
        }
    }

    public static T MakeStructFromBytes<T>(byte[] bytes)
    {
        int size = Marshal.SizeOf(typeof(T));
        IntPtr ptr = Marshal.AllocHGlobal(size);        // ���� �޸� �Ҵ�

        Marshal.Copy(bytes, 0, ptr, size);              // ����

        // �޸𸮷κ��� T�� ����ü�� ��ȯ
        T tStruct = (T)Marshal.PtrToStructure(ptr, typeof(T));
        Marshal.FreeHGlobal(ptr);       // �Ҵ�� �޸� ����
        return tStruct;                 // ��ȯ�� �� ��ȯ
    }
}
