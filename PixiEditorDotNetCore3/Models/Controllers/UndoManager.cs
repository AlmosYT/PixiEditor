﻿using PixiEditorDotNetCore3.Models.DataHolders;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace PixiEditor.Models.Controllers
{
    public static class UndoManager
    {
        public static StackEx<Change> UndoStack { get; set; } = new StackEx<Change>();
        public static StackEx<Change> RedoStack { get; set; } = new StackEx<Change>();
        private static bool _stopRecording = false;
        private static List<Change> _recordedChanges = new List<Change>();
        private static bool _lastChangeWasUndo = false;
        public static bool CanUndo
        {
            get
            {
                return UndoStack.Count > 0;
            }
        }
        public static bool CanRedo
        {
            get
            {
                return RedoStack.Count > 0;
            }
        }

        public static object MainRoot { get; set; }

        /// <summary>
        /// Sets object(root) in which undo properties are stored.
        /// </summary>
        /// <param name="root">Parent object.</param>
        public static void SetMainRoot(object root)
        {
            MainRoot = root;
        }

        /// <summary>
        /// Records changes, used to save multiple changes as one
        /// </summary>
        /// <param name="property">Record property name.</param>
        /// <param name="oldValue">Old change value.</param>
        /// <param name="newValue">New change value.</param>
        /// <param name="undoDescription">Description of change.</param>
        public static void RecordChanges(string property, object oldValue, object newValue, string undoDescription = "")
        {
            if (_stopRecording == false)
            {
                if (_recordedChanges.Count >= 2)
                {
                    _recordedChanges.RemoveAt(_recordedChanges.Count - 1);
                }
                _recordedChanges.Add(new Change(property, oldValue, newValue, undoDescription));

            }
        }

        /// <summary>
        /// Stops recording changes and saves it as one.
        /// </summary>
        public static void StopRecording()
        {
            _stopRecording = true;
            if (_recordedChanges.Count > 0)
            {
                Change changeToSave = _recordedChanges[0];
                changeToSave.NewValue = _recordedChanges.Last().OldValue;
                AddUndoChange(changeToSave);
                _recordedChanges.Clear();
            }
            _stopRecording = false;
        }

        public static void AddUndoChange(Change change)
        {
            if (_lastChangeWasUndo == false && RedoStack.Count > 0) //Cleares RedoStack if las move wasn't redo or undo and if redo stack is greater than 0
            {
                RedoStack.Clear();
            }
            _lastChangeWasUndo = false;
            UndoStack.Push(change);
            Debug.WriteLine("UndoStackCount: " + UndoStack.Count + " RedoStackCount: " + RedoStack.Count);
        }
        /// <summary>
        /// Adds property change to UndoStack
        /// </summary>
        /// <param name="property">Changed property name.</param>
        /// <param name="oldValue">Old value of property.</param>
        /// <param name="newValue">New value of property.</param>
        /// <param name="undoDescription">Description of change.</param>
        public static void AddUndoChange(string property, object oldValue, object newValue, string undoDescription = "")
        {
            if (_lastChangeWasUndo == false && RedoStack.Count > 0) //Cleares RedoStack if las move wasn't redo or undo and if redo stack is greater than 0
            {
                RedoStack.Clear();
            }
            _lastChangeWasUndo = false;
            UndoStack.Push(new Change(property, oldValue, newValue, undoDescription));
            Debug.WriteLine("UndoStackCount: " + UndoStack.Count + " RedoStackCount: " + RedoStack.Count);
        }

        /// <summary>
        /// Sets top property in UndoStack to Old Value
        /// </summary>
        public static void Undo()
        {
            _lastChangeWasUndo = true;
            PropertyInfo propInfo = MainRoot.GetType().GetProperty(UndoStack.Peek().Property);
            propInfo.SetValue(MainRoot, UndoStack.Peek().OldValue);
            RedoStack.Push(UndoStack.Pop());
        }

        /// <summary>
        /// Sets top property from RedoStack to old value
        /// </summary>
        public static void Redo()
        {
            _lastChangeWasUndo = true;
            PropertyInfo propinfo = MainRoot.GetType().GetProperty(RedoStack.Peek().Property);
            propinfo.SetValue(MainRoot, RedoStack.Peek().NewValue);
            UndoStack.Push(RedoStack.Pop());

        }
    }
}
