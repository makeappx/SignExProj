﻿#pragma checksum "..\..\ProcessChoose.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "F6E23286BB1B6143694D502A126BB08F58AF0665AB4A005DFE599CA32A376B8A"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using SignExProj;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace SignExProj {
    
    
    /// <summary>
    /// ProcessChoose
    /// </summary>
    public partial class ProcessChoose : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 20 "..\..\ProcessChoose.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView ProcessesList;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\ProcessChoose.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label Name;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\ProcessChoose.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label ID;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\ProcessChoose.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label Responding;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\ProcessChoose.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label Start;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\ProcessChoose.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label Readonly;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/SignExProj;component/processchoose.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ProcessChoose.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.ProcessesList = ((System.Windows.Controls.ListView)(target));
            
            #line 20 "..\..\ProcessChoose.xaml"
            this.ProcessesList.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ProcessesList_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 23 "..\..\ProcessChoose.xaml"
            ((System.Windows.Controls.Label)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.CloseP);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 24 "..\..\ProcessChoose.xaml"
            ((System.Windows.Controls.Label)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.KillP);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Name = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.ID = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            this.Responding = ((System.Windows.Controls.Label)(target));
            return;
            case 7:
            this.Start = ((System.Windows.Controls.Label)(target));
            return;
            case 8:
            this.Readonly = ((System.Windows.Controls.Label)(target));
            return;
            case 9:
            
            #line 46 "..\..\ProcessChoose.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

