//-----------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
//-----------------------------------------------------------------------------------------------------
namespace GGPlatform.ExcelManagers
{
    //-----------------------------------------------------------------------------------------------------
    public static class ExceptionStrings
    {
        public static string[] ExceptionMessages = 
        { 
            "���������� ������� ��� ������� ��� �������� Excel ���������!",
            "������ �������� Excel ���������!\n�������� ���������� MS Excel �� ����������� �� ������ ����������",
            "���������� ������� ��� ����� � ������� ���������!",
            "������� ����� ������� ��������� �� ���������������",
            "������ �������� Excel ���������!\n�������� ����������� ���������� OpenXML �� ������ ����������",
            "��������� ������ ��������� ������ Excel 2007 � �������!",
        };
    }
    //-----------------------------------------------------------------------------------------------------
    public class ExceptionManeger : Exception
    {
        private string MessageString;
        //-----------------------------------------------------------------------------------------------------

        public string PMessageString
        {
            get { return MessageString; }
        }
        //-----------------------------------------------------------------------------------------------------
        public ExceptionManeger(string InputMessageString)
        {
            MessageString = InputMessageString;
        }
        //-----------------------------------------------------------------------------------------------------
    }
    //-----------------------------------------------------------------------------------------------------
}
//-----------------------------------------------------------------------------------------------------
