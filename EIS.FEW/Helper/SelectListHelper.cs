using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace EIS.FEW.HtmlHelpers
{
    public static class SelectListHelper
    {


       
        /// <typeparam name="T"></typeparam>
        /// <param name="helper">The helper.</param>
        /// <param name="valueName">Name of the value.</param>
        /// <param name="textName">Name of the text.</param>
        /// <param name="valueSelected">The value selected.</param>
        /// <param name="valueDefault">The value default.</param>
        /// <param name="textDefault">The text default.</param>
        /// <returns>SelectList.</returns>
        /// <exception cref="System.Exception">valueName not of  + typeof(T).Name</exception>
        public static SelectList ToSelectList<T>(this IEnumerable<T> helper, string valueName, string textName, string valueSelected = "",
            string valueDefault = "", string textDefault = "")
        {
            var selectListItems = new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Value = valueDefault,
                    Text = textDefault
                }
            };
            var props = TypeDescriptor.GetProperties(typeof(T));

            PropertyDescriptor prop = props.Find(valueName, true);
            if (prop == null)
            {
                throw new Exception(string.Format("{0} not of " + typeof(T).Name, valueName));
            }
            int valueIndex = props.IndexOf(prop);

            prop = props.Find(textName, true);
            if (prop == null)
            {
                throw new Exception(string.Format("{0} not of " + typeof(T).Name, textName));
            }
            int textIndex = props.IndexOf(prop);

            selectListItems.AddRange(from item in helper
                                     let value = Convert.ToString(props[valueIndex].GetValue(item))
                                     let selected = !string.IsNullOrEmpty(valueSelected) && valueSelected.Equals(value, StringComparison.OrdinalIgnoreCase)
                                     select new SelectListItem()
                                     {
                                         Value = value,
                                         Selected = selected,
                                         Text = Convert.ToString(props[textIndex].GetValue(item))
                                     });

            return new SelectList(selectListItems, "Value", "Text", valueSelected);
        }

        /// <typeparam name="T"></typeparam>
        /// <param name="helper">The helper.</param>
        /// <param name="valueName">Name of the value.</param>
        /// <param name="textName">Name of the text.</param>
        /// <param name="textDefault">The text default.</param>
        /// <returns>SelectList.</returns>
        /// <exception cref="System.Exception">valueName not of  + typeof(T).Name</exception>
        public static SelectList ToSelectList<T>(this IEnumerable<T> helper, string valueName, string textName, string textDefault = "")
        {
            return ToSelectList<T>(helper, valueName, textName, textDefault, true);
        }
        
        /// <typeparam name="T"></typeparam>
        /// <param name="helper">The helper.</param>
        /// <param name="valueName">Name of the value.</param>
        /// <param name="textNames">The text names.</param>
        /// <param name="format">The format.</param>
        /// <param name="textDefault">The text default.</param>
        /// <returns>SelectList.</returns>
        /// <exception cref="System.Exception"></exception>
        public static SelectList ToSelectList<T>(this IEnumerable<T> helper, string valueName, string[] textNames, string format, string textDefault = "")
        {
            var selectListItems = new List<SelectListItem>();
            var props = TypeDescriptor.GetProperties(typeof(T));
            if (props[valueName] == null)
            {
                throw new Exception(string.Format("{0} not of " + typeof(T).Name, valueName));
            }
            foreach (var textName in textNames)
            {
                if (props[textName] == null)
                {
                    throw new Exception(string.Format("{0} not of " + typeof(T).Name, textName));
                }
            }

            selectListItems.AddRange(from item in helper
                                     let value = Convert.ToString(props[valueName].GetValue(item))
                                     select new SelectListItem()
                                     {
                                         Value = value,
                                         Text = BuildText(props, item, textNames, format)
                                     });

            selectListItems.Insert(0, new SelectListItem
            {
                Selected = true,
                Value = "",
                Text = textDefault
            });

            return new SelectList(selectListItems, "Value", "Text", "");
        }

        private static string BuildText<T>(PropertyDescriptorCollection props, T item, string[] textNames, string format)
        {
            string[] paramsList = new string[textNames.Length];
            for (int i = 0; i < textNames.Length; i++)
            {
                paramsList[i] = Convert.ToString(props[textNames[i]].GetValue(item));
            }
            return string.Format(format, paramsList);
        }
        
        /// <typeparam name="T"></typeparam>
        /// <param name="helper">The helper.</param>
        /// <param name="valueName">Name of the value.</param>
        /// <param name="textName">Name of the text.</param>
        /// <param name="textDefault">The text default.</param>
        /// <param name="hasDefault">if set to <c>true</c> [has default].</param>
        /// <returns>SelectList.</returns>
        /// <exception cref="System.Exception"></exception>
        public static SelectList ToSelectList<T>(this IEnumerable<T> helper, string valueName, string textName, string textDefault = "", bool hasDefault = true)
        {
            var selectListItems = new List<SelectListItem>();
            var props = TypeDescriptor.GetProperties(typeof(T));
            if (props[valueName] == null)
            {
                throw new Exception(string.Format("{0} not of " + typeof(T).Name, valueName));
            }
            if (props[textName] == null)
            {
                throw new Exception(string.Format("{0} not of " + typeof(T).Name, textName));
            }

            selectListItems.AddRange(from item in helper
                                     let value = Convert.ToString(props[valueName].GetValue(item))
                                     select new SelectListItem()
                                     {
                                         Value = value,
                                         Text = Convert.ToString(props[textName].GetValue(item))
                                     });

            if (hasDefault)
            {
                selectListItems.Insert(0, new SelectListItem
                                                            {
                                                                Selected = true,
                                                                Value = "",
                                                                Text = textDefault
                                                            });
            }

            return new SelectList(selectListItems, "Value", "Text");
        }

   
        /// <param name="helper">The helper.</param>
        /// <param name="valueName">Name of the value.</param>
        /// <param name="textName">Name of the text.</param>
        /// <param name="textDefault">The text default.</param>
        /// <returns>SelectList.</returns>
        /// <exception cref="System.Exception"></exception>
        public static SelectList ToSelectList(this DataTable helper, string valueName, string textName, string textDefault = "")
        {
            var selectListItems = new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Selected = true,
                    Value = "",
                    Text = textDefault
                }
            };
            if (!helper.Columns.Contains(valueName))
            {
                throw new Exception(string.Format("{0} not of Table", valueName));
            }
            if (!helper.Columns.Contains(textName))
            {
                throw new Exception(string.Format("{0} not of Table", textName));
            }

            selectListItems.AddRange(from item in helper.Select()
                                     select new SelectListItem()
                                     {
                                         Value = ((DataRow)item)[valueName].ToString(),
                                         Text = ((DataRow)item)[textName].ToString()
                                     });

            return new SelectList(selectListItems, "Value", "Text");
        }

     
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="selectList">The select list.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>MvcHtmlString.</returns>
        public static MvcHtmlString DropDownList(this HtmlHelper htmlHelper, string name, string value, IEnumerable<SelectListItem> selectList, object htmlAttributes = null)
        {
            var selectListItems = selectList as SelectListItem[] ?? selectList.ToArray();
            if (selectListItems.Any())
            {
                foreach (var selectListItem in selectListItems)
                {
                    if (selectListItem.Value == value)
                    {
                        selectListItem.Selected = true;
                    }
                    else
                    {
                        selectListItem.Selected = false;
                    }
                }
            }
            return htmlHelper.DropDownList(name, selectListItems, htmlAttributes);
        }


    }
}