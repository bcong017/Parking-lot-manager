﻿#pragma checksum "..\..\BaoCaoDoanhThu.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "881FC21E0AE068CBEB309FE4C7F4F705BA66FC2A782B961AF1AD846EE1822F21"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using LiveCharts.Wpf;
using MaterialDesignThemes.MahApps;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using QLBaiDoXe;
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


namespace QLBaiDoXe {
    
    
    /// <summary>
    /// BaoCaoDoanhThu
    /// </summary>
    public partial class BaoCaoDoanhThu : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 16 "..\..\BaoCaoDoanhThu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox YearTextbox;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\BaoCaoDoanhThu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock YearTextBlock;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\BaoCaoDoanhThu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock IncomeTextbox;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\BaoCaoDoanhThu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button GetReportButton;
        
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
            System.Uri resourceLocater = new System.Uri("/QLBaiDoXe;component/baocaodoanhthu.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\BaoCaoDoanhThu.xaml"
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
            this.YearTextbox = ((System.Windows.Controls.TextBox)(target));
            
            #line 16 "..\..\BaoCaoDoanhThu.xaml"
            this.YearTextbox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.YearTextbox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.YearTextBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.IncomeTextbox = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.GetReportButton = ((System.Windows.Controls.Button)(target));
            
            #line 42 "..\..\BaoCaoDoanhThu.xaml"
            this.GetReportButton.Click += new System.Windows.RoutedEventHandler(this.GetReportButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

