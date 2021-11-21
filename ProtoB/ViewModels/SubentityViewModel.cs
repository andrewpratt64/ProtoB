using DynamicData.Binding;
using ProtoB.Models;
using ProtoB.Native;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoB.ViewModels
{
    // TODO: Consider having a cache for some non-blittable properties (strings, collections, etc.)
    //      ^ So that a new object dosen't need to be constructed every time an accessor is called
    //      when the state of the SubentityModel hasn't changed at all
    // TODO: Use Rosyln to automate generating code for the properties, they currently take up a lot
    //      of space and are nearly identical
    // TODO: Lean more towards an RAII-esc paradigm; Don't create the view model for every child until
    //      it's needed, such as when it's been expanded in the treeview
    public class SubentityViewModel
    {
        /// <summary>Subentity this viewmodel encapsulates</summary>
        private unsafe SubentityModel* m_model;

        private ObservableCollection<SubentityViewModel>? _children;


        /// <summary>Name of the subentity used in the editor</summary>
        /// <remarks>Name should be unique; no two subentities</remarks>
        public string EditorName
        {
            get
            {
                unsafe { return new string(m_model->editorName); }
            }
            set
            {
                unsafe
                {
                    Native.Util.AssignStringToCharArray(
                        m_model->editorName,
                        Native.Util.SUBENTITY_MODEL_EDITOR_NAME_MAXLEN,
                        value
                    );
                }
            }
        }


        /// <summary>Name of the subentity</summary>
        // TODO: Should this be nullable?
        public string TrueName
        {
            get
            {
                unsafe { return new string(m_model->trueName); }
            }
            set
            {
                unsafe
                {
                    Native.Util.AssignStringToCharArray(
                        m_model->trueName,
                        Native.Util.ENTITY_MODEL_NAME_MAXLEN,
                        value
                    );
                }
            }
        }


        // TODO: public string Type
        // TODO: public string TypeHash


        /// <summary>Unique identifier for this subentity</summary>
        public ulong EntityId
        {
            get
            {
                unsafe { return m_model->entityid; }
            }
            set
            {
                unsafe{ m_model->entityid = value; }
            }
        }


        // TODO: Document
        /// <summary>Editor only</summary>
        public bool EditorOnly
        {
            get
            {
                unsafe { return m_model->flags.HasFlag(Native.SubentityFlags.EDITOR_ONLY); }
            }
            set
            {
                unsafe
                {
                    if (value)
                        m_model->flags |= Native.SubentityFlags.EDITOR_ONLY;
                    else
                        m_model->flags &= ~Native.SubentityFlags.EDITOR_ONLY;
                }
            }
        }


        // TODO: Document
        /// <summary>Exposed</summary>
        public bool Exposed
        {
            get
            {
                unsafe { return m_model->flags.HasFlag(Native.SubentityFlags.EXPOSED); }
            }
            set
            {
                unsafe
                {
                    if (value)
                        m_model->flags |= Native.SubentityFlags.EXPOSED;
                    else
                        m_model->flags &= ~Native.SubentityFlags.EXPOSED;
                }
            }
        }


        /// <summary>Collection of subentities that are parented to this subentity</summary>
        public ObservableCollection<SubentityViewModel>? Children
        {
            get
            {
                // Initialize the children collection if it hasn't been done yet
                if (_children == null)
                    PopulateChildren();
                return _children;
            }
            set
            {
                _children = value;
            }
        }


        public unsafe SubentityViewModel(SubentityModel* model)
        {
            m_model = model;
        }


        public SubentityViewModel()
        {
            unsafe
            {
                m_model = ProtoBCpp.newSubentityModel(null);
            }
        }


        /// <summary>Populates the Children property using the subentity model</summary>
        /// <remarks>Call this AFTER m_model has been initialized</remarks>
        private void PopulateChildren()
        {
            _children = new();
            // Create a new container for the children field

            unsafe
            {
                // Append a new SubentityViewModel for each child
                for (int i = 0; i < m_model->children.size; i++)
                {
                    _children.Add(
                        new SubentityViewModel(
                            *(SubentityModel**)(m_model->children.data + i)
                        )
                    );
                }
            }

            // TODO: Register event handlers here
        }
    }
}
