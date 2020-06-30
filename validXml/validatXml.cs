using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace GGPlatform.validXml
{
    public class XmlNodeError
    {
        public XmlNodeError(XElement inObj, ValidationEventArgs inExp)
        {
            this.obj = inObj; //кусок ХМЛ содержащий ошибку
            this.exp = inExp; //выявленная ошибка

            this.im_pol = obj.Name.LocalName;        //имя поля в котором ошибка
            this.bas_el = obj.Parent.Name.LocalName; //имя родительского тэга в котором ошибка

            /*   if (bas_el == "ZAP")
               {
                   n_zap = (from yy in obj.Parent.Elements("N_ZAP")
                            select yy.Value).First();
               }
               if (bas_el == "PACIENT")
               {
                   n_zap = (from yy in obj.Parent.Parent.Elements("N_ZAP") //через родителя получу адрес узла
                            select yy.Value).First();
               }
               if (bas_el == "SLUCH")
               {
                   idcase = (from yy in obj.Parent.Elements("IDCASE")      //через родителя получу адрес узла
                             select yy.Value).First();
                   n_zap = (from yy in obj.Parent.Parent.Elements("N_ZAP") //через родителя получу адрес узла
                            select yy.Value).First();
               }
               if (bas_el == "USL")
               {
                   idserv = (from yy in obj.Parent.Elements("IDSERV")
                             select yy.Value).First();
                   idcase = (from yy in obj.Parent.Parent.Elements("IDCASE")
                             select yy.Value).First();
                   n_zap = (from yy in obj.Parent.Parent.Parent.Elements("N_ZAP")
                            select yy.Value).First();
               }*/
            comment = exp.Exception.Message;
        }

        ~XmlNodeError()
        {
            obj = null;
            exp = null;
        }
        public string im_pol { get; set; }
        public string bas_el { get; set; }
        public string comment { get; set; }
        public XElement obj { get; set; }
        public ValidationEventArgs exp { get; set; }
        public override string ToString()
        {
            return "В элемент е " + im_pol
                 + " базовым элементом которого является " + bas_el
                 + " произошла ошибка " + comment;
        }
    }  //XML узел документа  и содержание выявленной ошибки в нем

    public class XmlSchemaValidator
    {
        private bool isValidXml = true;
        private List<string> validationErrors; //возвращает список ошибок
        private List<XmlNodeError> nodeErrors; //возвращает список узлов и ошибок в этих узлах

        ~XmlSchemaValidator()
        {
            if (validationErrors != null)
            {
                validationErrors.Clear();
                validationErrors = null;
            }
            
            if (nodeErrors != null)
            {
                nodeErrors.Clear();
                nodeErrors = null;
            }            
        }
        public XmlSchemaValidator()
        {
            validationErrors = new List<string>();
            nodeErrors = new List<XmlNodeError>();
        }
        /// <summary>
        /// возвращает список выявленных ошибок в документе
        /// </summary>
        public String ValidationError
        {
            get
            {
                return String.Join(",\n", validationErrors.ToArray()); ;
            }
            private set
            {
                validationErrors.Add(value);
            }
        }
        /// <summary>
        /// возвращает список ХМЛ узлов с содержимым, в которых были выявленны ошибки и список ошибок
        /// </summary>
        public List<XmlNodeError> NodeErrors
        {
            get
            {
                return nodeErrors;
            }
        }
        /// <SUMMARY>
        /// идентификатор Валидного ХМЛ документа
        /// </SUMMARY>
        public bool IsValidXml
        {
            get
            {
                return this.isValidXml;
            }
        }
        /// <SUMMARY>
        /// метод проверяет валидность XML строки по схеме без добавления узла
        /// </SUMMARY>
        /// <PARAM name="xml">XML string</PARAM>
        /// <PARAM name="schemaNamespace">XML Schema Namespace</PARAM>
        /// <PARAM name="schemaUri">XML Schema filename</PARAM>
        /// <PARAM name="ifNeedNodeErrors">формировать ли список узлов в которых содержится ошибка или нет</PARAM>
        /// <RETURNS>bool</RETURNS>
        public bool ValidXmlDoc(string xmlValueOrxmlFile, string schemaNamespace, string schemaUri, bool ifNeedNodeErrors = false)
        {
            try
            {
                if (xmlValueOrxmlFile == null || xmlValueOrxmlFile.Length < 1)
                {
                    return false;
                }
                if (xmlValueOrxmlFile.Contains(":\\") && xmlValueOrxmlFile.Contains(".xml")) //если это путь к файлу, то проверяю его наличие
                {
                    if (!File.Exists(xmlValueOrxmlFile))
                        throw new Exception("Файл " + xmlValueOrxmlFile + " не найден");
                }

                StringReader srXml;
                if (xmlValueOrxmlFile.Contains(":\\") && xmlValueOrxmlFile.Contains(".xml"))
                    srXml = new StringReader(File.ReadAllText(xmlValueOrxmlFile, Encoding.ASCII));
                else
                    srXml = new StringReader(xmlValueOrxmlFile);
                return ValidXmlDoc(srXml, schemaNamespace, schemaUri, ifNeedNodeErrors);
            }
            catch (Exception ex)
            {
                this.ValidationError = ex.Message;
                return false;
            }
        }
        /// <SUMMARY>
        /// метод проверяет валидность XML документа по схеме без добавления узла
        /// </SUMMARY>
        /// <PARAM name="xml">XML document</PARAM>
        /// <PARAM name="schemaNamespace">XML Schema Namespace</PARAM>
        /// <PARAM name="schemaUri">XML Schema filename</PARAM>
        /// <PARAM name="ifNeedNodeErrors">формировать ли список узлов в которых содержится ошибка или нет</PARAM>
        /// <RETURNS>bool</RETURNS>/// 
        public bool ValidXmlDoc(XmlDocument xmlDoc, string schemaNamespace, string schemaUri, bool ifNeedNodeErrors = false)
        {
            try
            {
                if (xmlDoc == null)
                {
                    return false;
                }
                StringWriter sw = new StringWriter();
                XmlTextWriter xw = new XmlTextWriter(sw);
                xmlDoc.WriteTo(xw);
                string strXml = sw.ToString();
                StringReader srXml = new StringReader(strXml);
                return ValidXmlDoc(srXml, schemaNamespace, schemaUri, ifNeedNodeErrors);
            }
            catch (Exception ex)
            {
                this.ValidationError = ex.Message;
                return false;
            }
        }
        /// <SUMMARY>
        /// основной метод проверяет валидность XML документа по схеме без добавления узла
        /// </SUMMARY>
        /// <PARAM name="xml">StringReader containing xml</PARAM>
        /// <PARAM name="schemaNamespace">XML Schema Namespace</PARAM>
        /// <PARAM name="schemaUri">XML Schema Uri</PARAM>
        /// <PARAM name="ifNeedNodeErrors">формировать ли список узлов в которых содержится ошибка или нет</PARAM>
        /// <RETURNS>bool</RETURNS>
        private bool ValidXmlDoc(StringReader xml, string schemaNamespace, string schemaUri, bool ifNeedNodeErrors = false)
        {
            if (xml == null || schemaNamespace == null || schemaUri == null)
            {
                return false;
            }
            if (!File.Exists(schemaUri))
            {
                throw new Exception("Файл схемы вылидации " + schemaUri + " не найден");
            }
            isValidXml = true;
            XmlReader xmldoc = null;

            if (!ifNeedNodeErrors) //НЕ нужно формировать список узлов
            {
                XmlReaderSettings xmlSettings = null;
                try
                {
                    xmlSettings = new XmlReaderSettings();
                    xmlSettings.Schemas.Add(schemaNamespace, schemaUri);
                    xmlSettings.ValidationType = ValidationType.Schema;
                    xmlSettings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
                    xmldoc = XmlReader.Create(xml, xmlSettings);

                    while (xmldoc.Read()) { }

                    if (xml != null)
                    {
                        xml.Close();
                        xml.Dispose();
                        xml = null;
                    }
                    if (xmldoc != null)
                    {
                        xmldoc.Close();
                        xmldoc = null;

                    }
                    xmlSettings = null;
                    return isValidXml;
                }
                catch (Exception ex)
                {
                    this.ValidationError = ex.Message;
                    return false;
                }
                finally
                {
                    if (xml != null)
                    {
                        xml.Close();
                        xml.Dispose();
                        xml = null;
                    }                    
                    if (xmldoc != null)
                    {
                        xmldoc.Close();
                        xmldoc = null;

                    }                       
                    xmlSettings = null;                    
                }
            }
            else
            {
                try
                {
                    xmldoc = XmlReader.Create(xml);
                    XmlSchemaSet xsd = new XmlSchemaSet();
                    xsd.Add(schemaNamespace, schemaUri);
                    while (!xmldoc.EOF)
                    {
                        if (xmldoc.NodeType == XmlNodeType.Element)
                        {
                            XDocument node = XDocument.Parse(xmldoc.ReadOuterXml());
                            node.Validate(xsd, (sender, args) =>
                                                {
                                                    isValidXml = false;
                                                    nodeErrors.Add(new XmlNodeError(sender as XElement, args as ValidationEventArgs));
                                                }
                                         );
                            xmldoc.Read();
                        }
                        else xmldoc.Read();
                    }

                    xsd = null;                  
                    if (xml != null)
                    {
                        xml.Close();
                        xml.Dispose();
                        xml = null;
                    }
                    if (xmldoc != null)
                    {
                        xmldoc.Close();
                        xmldoc = null;
                    }

                    return isValidXml;
                }
                catch
                {
                    if (xml != null)
                    {
                        xml.Close();
                        xml.Dispose();
                        xml = null;
                    }
                    if (xmldoc != null)
                    {
                        xmldoc.Close();
                        xmldoc = null;
                    }
                    return false;
                }
                finally
                {
                    if (xml != null)
                    {
                        xml.Close();
                        xml.Dispose();
                        xml = null;
                    }
                    if (xmldoc != null)
                    {
                        xmldoc.Close();
                        xmldoc = null;
                    }
                }
            }



        }
        /// <SUMMARY>
        /// метод проверяет валидность документа по схеме без добавления узла
        /// </SUMMARY>
        /// <PARAM name="sender"></PARAM>
        /// <PARAM name="args"></PARAM>
        private void ValidationCallBack(object sender, ValidationEventArgs args)
        {
            isValidXml = false; //файл не Валидный
            this.ValidationError = args.Message;  //добавлю ошибку в список
        }

        /* public string GetCurrentNodeAsString(XmlReader reader)
         {
             string result = "";
             switch (reader.NodeType)
             {
                 case XmlNodeType.Element:
                     result = string.Format("<{0}>", reader.Name);
                     break;
                 case XmlNodeType.Text:
                     result = reader.Value;
                     break;
                 case XmlNodeType.CDATA:
                     result = string.Format("<![CDATA[{0}]]>", reader.Value);
                     break;
                 case XmlNodeType.ProcessingInstruction:
                     result = string.Format("<?{0} {1}?>", reader.Name, reader.Value);
                     break;
                 case XmlNodeType.Comment:
                     result = string.Format("<!--{0}-->", reader.Value);
                     break;
                 case XmlNodeType.XmlDeclaration:
                     result = string.Format("<?{0} {1}?>", reader.Name, reader.Value);
                     break;
                 case XmlNodeType.Document:
                     break;
                 case XmlNodeType.DocumentType:
                     result = string.Format("<!DOCTYPE {0} [{1}]", reader.Name, reader.Value);
                     break;
                 case XmlNodeType.EndElement:
                     result = string.Format("</{0}>", reader.Name);
                     break;
                 default:
                     break;
             }
             return result;
         }*/

    }

}
