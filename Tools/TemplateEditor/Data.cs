using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Collections;


namespace TemplateEditor
{
    [ServerFrame.Editor.CDataEditorAttribute(".cfg")]
    public class ExportConfig
    {
        [System.ComponentModel.Category("excel导入导出")]
        [System.ComponentModel.DisplayName("默认打开模版文件路径")]
        [ServerFrame.Config.DataValueAttribute("OpenTemplatePath")]
        public string OpenTemplatePath { get; set; }

        [System.ComponentModel.Category("excel导入导出")]
        [System.ComponentModel.DisplayName("excel导出目录")]
        [ServerFrame.Config.DataValueAttribute("ExportExcelPath")]
        public string ExportExcelPath { get; set; }



        [System.ComponentModel.Category("excel导入导出")]
        [System.ComponentModel.DisplayName("默认打开excel目录")]
        [ServerFrame.Config.DataValueAttribute("OpenExcelPath")]
        public string OpenExcelPath { get; set; }

        [System.ComponentModel.Category("excel导入导出")]
        [System.ComponentModel.DisplayName("文件导出默认打开路径")]
        [ServerFrame.Config.DataValueAttribute("ExportFilePath")]
        public string ExportFilePath { get; set; }



        string mSearchDefalutFolder = "../DemaciaGame/Template/";
        [System.ComponentModel.Category("文件工具设置")]
        [System.ComponentModel.DisplayName("模版默认路径")]
        [ServerFrame.Config.DataValueAttribute("SearchDefalutFolder")]
        public string SearchDefalutFolder { get { return mSearchDefalutFolder; } set { mSearchDefalutFolder = value; } }

    }
}
