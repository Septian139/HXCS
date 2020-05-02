using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace HXCS.Base
{
   

    class HXCS
    {
        private Type[] modelType;
        public HXCS (params Type[] model)
        {
            modelType = model;
        }

        public T ExecuteSelect<T>() where T: Model
        {
            string ret = "SELECT ";
            Type t = typeof(T);

            string mainTblObj = t.Name;
            string column = "";

            PropertyInfo[] mis = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            object obj = Activator.CreateInstance(t);
            foreach (PropertyInfo pi in mis)
            {
                if(pi.Name == "Name")
                {
                    pi.SetValue(obj, "Septian", null);
                }
                ret += "\n" + pi.Name;
            }

           // obj.GetType().InvokeMember("Name",BindingFlags.Public |BindingFlags.Instance | BindingFlags.SetProperty, Type.DefaultBinder, obj, "Alika");
            return (T)obj;

        }

        /// <summary>
        /// Create Class Model into Database Table Object
        /// </summary>
        /// <returns></returns>
        public string InitTable()
        {
            string p = "/* HXCS Auto Generate Query */";
            string relationshipConstraint = "";

            foreach(Type t in modelType)
            {
                object[] objCls = t.GetCustomAttributes(true);
                bool isValid = false;
                foreach (object obj in objCls)
                {
                    Table nl = obj as Table;
                    if (nl != null)
                    {
                        isValid = true;
                        break;
                    }
                }

                if (!isValid)
                {
                    return "";
                }

                PropertyInfo[] mis = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                p += "\n\nIF NOT EXISTS (SELECT * FROM sys.tables WHERE name = '" + t.Name + "')";
                p += "\n\tCREATE TABLE [dbo].[" + t.Name + "] (\n\t\t[id] [int] IDENTITY(1,1) NOT NULL";
                foreach (PropertyInfo pi in mis)
                {
                    object[] objAttrs = pi.GetCustomAttributes(true);
                    bool con = true;
                    bool canNull = true;
                    bool isUnique = false;
                    int length = 0;
                    foreach (object obj in objAttrs)
                    {
                        Length ln = obj as Length;
                        if (ln != null)
                        {
                            length = ln.Size;
                        }

                        NotNull nl = obj as NotNull;
                        if (nl != null)
                        {
                            canNull = false;
                        }

                        Unique un = obj as Unique;
                        if (un != null)
                        {
                            isUnique = true;
                        }

                        ForeignKey fk = obj as ForeignKey;
                        if (fk != null)
                        {
                            if (pi.GetMethod.ReturnType.BaseType.Equals(typeof(Model)))
                            {
                                relationshipConstraint += "\n\nALTER TABLE " + t.Name;
                                relationshipConstraint += "\nADD CONSTRAINT FK_" + pi.Name + t.Name;
                                relationshipConstraint += "\nFOREIGN KEY (" + pi.Name + "_id)";
                                relationshipConstraint += "\nREFERENCES " + pi.Name + "(id)\nGO";

                                p += ",\n\t\t[" + pi.Name + "_id] [int]";
                                if (!canNull)
                                {
                                    p += " NOT NULL";
                                }
                                if (isUnique)
                                {
                                    p += " UNIQUE";
                                }
                                con = false;
                            }
                            else
                            {
                                if (pi.GetMethod.ReturnType.BaseType.Equals(typeof(Model)))
                                {
                                    throw new HXCSException("HXCS.Base.Model Class cannot be used as a Foreign Key");
                                }
                                else
                                {
                                    throw new HXCSException("Foreign Key must be a Class which is inherit HXCS.Base.Model Class");
                                }
                                   
                            }

                        }
                    }
                    if (con)
                    {
                        string ret = "";
                        if (pi.GetMethod.ReturnType.Equals(typeof(int)))
                        {
                            ret = "[int]";
                        }
                        else if (pi.GetMethod.ReturnType.Equals(typeof(double)))
                        {
                            ret = "[float]";
                        }
                        else if (pi.GetMethod.ReturnType.Equals(typeof(string)))
                        {
                            ret = "[varchar]";
                            if (length == 0)
                            {
                                ret += "(max)";
                            }
                            
                        }
                        else if (pi.GetMethod.ReturnType.Equals(typeof(DateTime)))
                        {
                            ret = "[datetime]";
                        }
                        else if (pi.GetMethod.ReturnType.Equals(typeof(bool)))
                        {
                            ret = "[int]";
                        }
                        else
                        {
                            if (pi.GetMethod.ReturnType.IsEnum)
                            {
                                ret = "[int]";
                            }
                            else
                            {
                                if (pi.GetMethod.ReturnType.BaseType.Equals(typeof(Model)))
                                {
                                    throw new HXCSException("Are you forget to add 'ForeignKey' Attribute?: " + pi.GetMethod.ReturnType.BaseType.ToString() + "(" + pi.Name + ")");
                                }
                                else
                                {
                                    throw new HXCSException("Data type not supported: " + pi.GetMethod.ReturnType.BaseType.ToString() + "(" + pi.Name + ")");
                                }
                            }
                        }

                        p += ",\n\t\t[" + pi.Name + "] " + ret;
                        if(length > 0)
                        {
                            p += "(" + length.ToString() + ")";
                        }
                        if (!canNull)
                        {
                            p += " NOT NULL";
                        }
                        if (isUnique)
                        {
                            p += " UNIQUE";
                        }
                    }
                }
                p += "\n\t\tCONSTRAINT PK_" + t.Name + " PRIMARY KEY CLUSTERED ([id] asc)";
                p += "\n\t)\nGO";
            }
            
            
            return p + relationshipConstraint;
        }

    }
}
