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
using System.ComponentModel;
using System.Configuration.Install;
using System.Security;
using System.Security.Policy;

namespace CustomSetupActions
{
    [RunInstaller(true)]
    public partial class CASPolicyInstaller : Installer
    {
        public CASPolicyInstaller()
        {
            InitializeComponent();
        }

        public override void Install(System.Collections.IDictionary stateSaver)
        {
            PolicyLevel ent;
            PolicyLevel mach;
            PolicyLevel user;
            string sAssemblyPath = this.Context.Parameters["custassembly"];
            //string sAssemblyPath = this.Context.Parameters["XWord.dll"];
            System.Collections.IEnumerator policies = SecurityManager.PolicyHierarchy();
            policies.MoveNext();
            ent = (PolicyLevel)policies.Current;
            policies.MoveNext();
            mach = (PolicyLevel)policies.Current;
            policies.MoveNext();
            user = (PolicyLevel)policies.Current;

            PermissionSet fullTrust = user.GetNamedPermissionSet("FullTrust");
            PolicyStatement statement = new PolicyStatement(fullTrust, PolicyStatementAttribute.Nothing);
            UrlMembershipCondition condition = new UrlMembershipCondition(sAssemblyPath);
            CodeGroup group = new UnionCodeGroup(condition, statement);
            group.Name = "TestWordAddInCS";
            user.RootCodeGroup.AddChild(group);
            SecurityManager.SavePolicy();
            
            base.Install(stateSaver);
        }
    }

   
}