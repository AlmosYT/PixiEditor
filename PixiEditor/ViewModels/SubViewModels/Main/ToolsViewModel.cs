﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using PixiEditor.Helpers;
using PixiEditor.Models.Tools;
using PixiEditor.Models.Tools.Tools;

namespace PixiEditor.ViewModels.SubViewModels.Main
{
    public class ToolsViewModel : SubViewModel<ViewModelMain>
    {
        private Cursor toolCursor;

        public RelayCommand SelectToolCommand { get; set; } // Command that handles tool switching.

        public RelayCommand ChangeToolSizeCommand { get; set; }

        public Tool LastActionTool { get; private set; }

        public ObservableCollection<Tool> ToolSet { get; set; }

        public Cursor ToolCursor
        {
            get => toolCursor;
            set
            {
                toolCursor = value;
                RaisePropertyChanged("ToolCursor");
            }
        }

        public ToolsViewModel(ViewModelMain owner)
            : base(owner)
        {
            SelectToolCommand = new RelayCommand(SetTool, Owner.DocumentIsNotNull);
            ChangeToolSizeCommand = new RelayCommand(ChangeToolSize);

            ToolSet = new ObservableCollection<Tool>
            {
                new MoveViewportTool(), new MoveTool(), new PenTool(), new SelectTool(), new FloodFill(), new LineTool(),
                new CircleTool(), new RectangleTool(), new EraserTool(), new ColorPickerTool(), new BrightnessTool(),
                new ZoomTool()
            };
            SetActiveTool(ToolType.Move);
        }

        public void SetActiveTool(ToolType tool)
        {
            Tool foundTool = ToolSet.First(x => x.ToolType == tool);
            SetActiveTool(foundTool);
        }

        public void SetActiveTool(Tool tool)
        {
            Tool activeTool = ToolSet.FirstOrDefault(x => x.IsActive);
            if (activeTool != null)
            {
                activeTool.IsActive = false;
            }

            tool.IsActive = true;
            LastActionTool = Owner.BitmapManager.SelectedTool;
            Owner.BitmapManager.SetActiveTool(tool);
            SetToolCursor(tool.ToolType);
        }

        public void SetTool(object parameter)
        {
            SetActiveTool((ToolType)parameter);
        }

        private void ChangeToolSize(object parameter)
        {
            int increment = (int)parameter;
            int newSize = Owner.BitmapManager.ToolSize + increment;
            if (newSize > 0)
            {
                Owner.BitmapManager.ToolSize = newSize;
            }
        }

        private void SetToolCursor(ToolType tool)
        {
            if (tool != ToolType.None)
            {
                ToolCursor = Owner.BitmapManager.SelectedTool.Cursor;
            }
            else
            {
                ToolCursor = Cursors.Arrow;
            }
        }
    }
}