﻿using System;
using System.Collections.Generic;
using Avalonia.Media;
using Avalonia.Input;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Controls.Primitives;

namespace WalletWasabi.Gui.Controls.LockScreen
{
    internal class SlideLock : UserControl
    {
        private Thumb DragThumb;
        private TranslateTransform DragThumbTransform;
        private bool UnlockInProgress = false;

        private double _offset = 0;
        private double Offset
        {
            get => _offset;
            set => OnOffsetChanged(value);
        }

        private const double Stiffness = 0.12d;

        public SlideLock()
        {
            InitializeComponent();

            this.DragThumb = this.FindControl<Thumb>("PART_DragThumb");
			DragThumbTransform = new TranslateTransform();

            DragThumb.DragCompleted += OnDragCompleted;
            DragThumb.DragDelta += OnDragDelta;
            DragThumb.DragStarted += OnDragStarted;
			DragThumb.RenderTransform = DragThumbTransform;

            Clock.Subscribe(OnClockTick);
        }

        private void OnOffsetChanged(double value)
        {
            _offset = value;
            DragThumbTransform.Y = _offset;
        }

        private void OnClockTick(TimeSpan CurrentTime)
        {
            if (!UnlockInProgress & Offset != 0)
            {
                Offset *= 1 - Stiffness;
            }
        }

        private void OnDragStarted(object sender, VectorEventArgs e)
        {
            UnlockInProgress = true;
        }

        private void OnDragDelta(object sender, VectorEventArgs e)
        {
            if (e.Vector.Y < 0)
                this.Offset = e.Vector.Y;
        }

        private void OnDragCompleted(object sender, VectorEventArgs e)
        {
            UnlockInProgress = false;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
