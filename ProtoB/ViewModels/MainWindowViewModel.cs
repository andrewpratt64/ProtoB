using Avalonia.Collections;
using Avalonia.Interactivity;
using ProtoB.Controls;
using ProtoB.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ProtoB.ViewModels
{
    public class MainWindowViewModel :
        ViewModelBase,
        IDisposable
    {
        private EntityViewModel m_rocco;
        private ObservableCollection<FooItem> _tmp = new();

        //private List<SubentityViewModel> m_root = new(1);

        public string Greeting => "Welcome to Avalonia!";

        public EntityViewModel Rocco => m_rocco;

        public IEnumerable<FooItem> Tmp => _tmp;


        public MainWindowViewModel()
        {
            _tmp.Add(new FooItem{});
            _tmp.Add(new FooItem{});
            _tmp.Add(new FooItem{});

            unsafe
            {
                m_rocco = new EntityViewModel(Native.ProtoBCpp.unittest_MakeAnEntity());
                //m_root.Add(m_rocco.RootSubentity);
                
            }
        }


        public void Dispose()
        {
            m_rocco.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
