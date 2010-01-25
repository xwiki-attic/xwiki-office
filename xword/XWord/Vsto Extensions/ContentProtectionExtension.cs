#region LGPL license

/*
 * See the NOTICE file distributed with this work for additional
 * information regarding copyright ownership.
 *
 * This is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License as
 * published by the Free Software Foundation; either version 2.1 of
 * the License, or (at your option) any later version.
 *
 * This software is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this software; if not, write to the Free
 * Software Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA
 * 02110-1301 USA, or see the FSF site: http://www.fsf.org.
 */

#endregion //license

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Word;
using Microsoft.Office.Core;
using XWiki;

namespace XWord.VstoExtensions
{
    /// <summary>
    /// Extension for protecting Word content with VSTO
    /// </summary>
    public static class ContentProtectionExtension
    {
        /// <summary>
        /// Protect a Document
        /// </summary>
        /// <param name="objDoc"></param>
        /// <param name="blnProtect"></param>
        public static void eProtectDoc(this Document objDoc, bool blnProtect)
        {
            object NoRest = false;
            object Passwod = "aaaaaa";
            WdProtectionType Type = blnProtect ? WdProtectionType.wdAllowOnlyReading : WdProtectionType.wdNoProtection;
            object UseIRM = false;
            object EnforceStyleLock = false;
            try
            {
                objDoc.Unprotect(ref Passwod);
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
            objDoc.Protect(Type, ref NoRest, ref Passwod, ref UseIRM, ref EnforceStyleLock);
        }

        /// <summary>
        /// Protect a Document with deleting all editable ranges
        /// </summary>
        /// <param name="objDoc"></param>
        /// <param name="blnProtect"></param>
        public static void eProtectDocClean(this Document objDoc, bool blnProtect)
        {
            object NoRest = false;
            object Passwod = "aaaaaa";
            WdProtectionType Type = blnProtect ? WdProtectionType.wdAllowOnlyReading : WdProtectionType.wdNoProtection;
            object UseIRM = false;
            object EnforceStyleLock = false;
            try
            {
                objDoc.Unprotect(ref Passwod);
            }
            catch(Exception ex) 
            {
                Log.Exception(ex);
            }
            Object everyone = WdEditorType.wdEditorEveryone;
            objDoc.DeleteAllEditableRanges(ref everyone);
            objDoc.Protect(Type, ref NoRest, ref Passwod, ref UseIRM, ref EnforceStyleLock);
            if (blnProtect == false)
            {
                objDoc.Content.eProtectRange(false);
            }
        }

        /// <summary>
        /// Protect Range
        /// </summary>
        /// <param name="objRng"></param>
        /// <param name="blnProtect"></param>
        /// <returns></returns>
        public static Range eProtectRange(this Range objRng, bool blnProtect)
        {
            Range Rng = null;
            if (objRng == null) return Rng;
            if (objRng.Start == objRng.End) return Rng;
            Object obj = WdEditorType.wdEditorEveryone;
            Object editor = objRng.Editors.Add(ref obj);
            if (blnProtect)
            {
                objRng.Editors.Item(ref obj).Delete();
            }
            return objRng;
        }



        /// <summary>
        /// Protect BookMark
        /// </summary>
        public static Range eProtectBookMark(this Document objDoc, string strBookMark, bool blnProtect)
        {
            Range Rng = null;
            if (strBookMark == null) return Rng;
            if (objDoc.Bookmarks.Exists(strBookMark))
            {
                object temp = strBookMark;
                Rng = objDoc.Bookmarks.get_Item(ref temp).Range;
            }
            if (Rng != null)
            {
                object obj = WdEditorType.wdEditorEveryone;
                Rng.Editors.Add(ref  obj);
                if (blnProtect) Rng.Editors.Item(ref obj).Delete();
            }
            return Rng;
        }
    }
}
