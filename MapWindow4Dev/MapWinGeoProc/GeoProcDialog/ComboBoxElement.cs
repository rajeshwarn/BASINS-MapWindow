using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace MapWinGeoProc.Dialogs
{

   /// <summary>
   /// A modular element that supports a textbox for users to enter information
   /// as part of a larger dialog control.
   /// </summary>
    public partial class ComboBoxElement : UserControl
    {
        #region variable declaration
        #region generic
        LightStatus m_Status;
        bool m_HaltOnError;
        bool m_HaltOnEmpty;
        StringBuilder m_HelpContent; // Stores help content to be displayed
        Image m_HelpImage;

        //Resizing 
        bool m_ResizeGripVisible;
        bool m_AllowGripResize;
        bool m_Resizing;
        int m_OldX;
        int m_MaxWidth;
        int m_MinWidth;

        // Help Stuff
        bool m_HelpButtonVisible; // necessary for changing this before the button exists
        string m_HelpTitle;
        string m_WikiAddress;
        Image m_DefaultImage;
        Image m_AlternateImage;


        /// <summary>
        /// Indicates the allowed values for the status of the element, illustrated by the light
        /// </summary>
        public enum LightStatus
        {
            /// <summary>
            /// Indicates that no value has been set for this yet.
            /// </summary>
            [Description("Indicates that no value has been set for this yet.")]
            Empty = 0,
            /// <summary>
            /// Indicates that the element parameter is ok and won't halt.
            /// </summary>
            [Description("Indicates that the element parameter is ok and won't halt.")]
            Ok = 1,
            /// <summary>
            /// Indicates that the element value will cause an error.
            /// </summary>
            [Description("Indicates that the element value will cause an error.")]
            Error = 2
        }
#endregion

        string[] m_ListItems;
        #endregion

        
        

        /// <summary>
        /// Creates a new instance of the TextElement class
        /// </summary>
        public ComboBoxElement()
        {
            InitializeComponent();
            
            m_AllowGripResize = true;
            m_HaltOnError = true;
            m_HaltOnEmpty = true;
            m_MinWidth = 10; // Large enough to keep the grip around
            m_HelpContent = new StringBuilder();
            m_HelpContent.Append("No help was provided for this element.");
            ttLight.SetToolTip(lblLight, "A value has not been specified yet.");
            ttHelp.SetToolTip(cmdHelp, "Help.");
            ttGrip.SetToolTip(lblGrip, "Resize Grip.");
            m_ResizeGripVisible = true; // These need to match the "defaults" it won't work right
            m_HelpButtonVisible = true;
            MinWidth = 100;
            Caption = "Text Input Box";
        }

        #region Events
        /// <summary>
        /// Fires when the inactive areas around the controls are clicked on the element.
        /// </summary>
        [Category("Action")]
        [Description("Fires when the inactive areas around the controls are clicked on the element.")]
        public event EventHandler Clicked;
        /// <summary>
        /// Called to fire the click event for this element
        /// </summary>
        /// <param name="e">A mouse event args thingy</param>
        protected new void OnClick(System.EventArgs e)
        {
            EventHandler handler = Clicked;
            if (handler != null)
            {
                handler(this, e);
            }
        }


        /// <summary>
        /// Public delegate for when someone presses the help button
        /// </summary>
        /// <param name="sender">The element sending the event</param>
        /// <param name="e">HelpPannelEventArgs with help title, text and an image</param>
        public delegate void HelpButtonHandler(object sender, HelpPannelEventArgs e);

        /// <summary>
        /// Fires when the help button is clicked.
        /// </summary>
        [Category("Action")]
        [Description("Fires when the Help button is clicked.")]
        public event HelpButtonHandler HelpButtonPressed;
        /// <summary>
        /// Called when the help button has been pressed
        /// </summary>
        /// <param name="e">A HelpPanelEventArgs parameter with a new help title, text and image.</param>
        protected virtual void OnHelpButtonPressed(HelpPannelEventArgs e)
        {
            HelpButtonHandler handler = HelpButtonPressed;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Fires when the the user enters a data entry region.  Changes help without opening panel.
        /// </summary>
        [Category("Action")]
        [Description("In child controls, this signifies that the data field is accessed.")]
        public event HelpButtonHandler HelpContextChanged;
        /// <summary>
        /// Called when the data entry field is entered to change a topic, but only if panel is visible
        /// </summary>
        /// <param name="e">A HelpPanelEventArgs parameter with a new help title, text and image.</param>
        protected virtual void OnHelpContextChanged(HelpPannelEventArgs e)
        {
            HelpButtonHandler handler = HelpContextChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }



        /// <summary>
        /// Delegate for the when someone starts resizing an element
        /// </summary>
        /// <param name="sender">The element being resized</param>
        public delegate void ResizeStartedHandler(object sender);

        /// <summary>
        /// Fires when the element is being resized.
        /// </summary>
        [Category("Action")]
        [Description("Fires when the element is being resized.")]
        public event ResizeStartedHandler ResizeStarted;

        /// <summary>
        /// Fires when someone uses the resize grip to resize the element
        /// </summary>
        protected virtual void OnResizeStarted()
        {
            ResizeStartedHandler handler = ResizeStarted;
            if (handler != null)
            {
                handler(this);
            }
        }
        /// <summary>
        /// Fires when the element is no longer being resized.
        /// </summary>
        /// <param name="sender"></param>
        public delegate void ResizeEndedHandler(object sender);
        /// <summary>
        /// Fires when the element is no longer being resized.
        /// </summary>
        [Category("Action")]
        [Description("Fires when the element is no longer being resized.")]
        public event ResizeEndedHandler ResizeEnded;
        /// <summary>
        /// Fires when someone has stopped resizing with the grip
        /// </summary>
        protected virtual void OnResizeEnded()
        {
            ResizeEndedHandler handler = ResizeEnded;
            if (handler != null)
            {
                handler(this);
            }
        }
        #endregion

        #region Properties
        #region Properties - Appearance

        /// <summary>
        /// Controls the caption for this element (the group box text)
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or Sets the text that will appear at the top of the group box.")]
        public string Caption
        {
            get
            {
                return groupBox1.Text;
            }
            set
            {
                groupBox1.Text = value;
            }
        }

       
        
        /// <summary>
        /// Gets or sets whether the status light is visible for this element.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or Sets whether or not the status light will be displayed.")]
        public bool LightVisible
        {
            get
            {
                return lblLight.Visible;
            }
            set
            {
                lblLight.Visible = value;
            }
        }
        /// <summary>
        /// Gets or sets the tool tip that shows when you hold the mouse over the light
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the tool tip that shows when you hold the mouse over the light.")]
        public string LightMessage
        {
            get
            {
                return ttLight.GetToolTip(lblLight);
            }
            set
            {
                ttLight.SetToolTip(lblLight, value);
            }
        }

        /// <summary>
        /// Indicates the current condition of this element.
        /// - Green = ok to continue
        /// - Yellow = not specified yet
        /// - Red = process halting error
        /// </summary>
        [Category("Appearance")]
        [Description("Shows the condition of the parameter being controled by this element.")]
        public LightStatus Status
        {
            get
            {
                return m_Status;
            }
            set
            {
                m_Status = value;
                lblLight.ImageIndex = (int)value;
            }
        }


        #endregion

        #region Properties - Behavior
        /// <summary>
        /// Gets or sets a boolean.  True if the user is allowed to adjust the width of the 
        /// element to better display longer filenames.
        /// </summary>
        [Category("Behavior")]
        [Description("Gets or sets a boolean.  True if the user is allowed to adjust the width of the element to better display longer filenames.")]
        public bool AllowGripResize
        {
            get
            {
                return m_AllowGripResize;
            }
            set
            {
                m_AllowGripResize = value;
            }
        }

        /// <summary>
        /// Boolean, true if either of the conditions that may prevent dialog completion are true 
        /// </summary>
        [Category("Behavior")]
        [Description("Get or Set.  Ideally, the dialog will check this property for each element and prevent completion if any of them are true.")]
        public bool Halt
        {
            get
            {
                if (m_Status == LightStatus.Ok) return false;
                if (m_HaltOnError == true && m_Status == LightStatus.Error) return true;
                if (m_HaltOnEmpty == true && m_Status == LightStatus.Empty) return true;
                return false;
            }
        }
        /// <summary>
        /// Gets or sets a boolean.  True if an unspecified condition will prevent dialog completion.
        /// </summary>
        [Category("Behavior")]
        [Description("Get or Set.  True if an unspecified condition will prevent dialog completion.")]
        public bool HaltOnEmpty
        {
            get
            {
                return m_HaltOnEmpty;
            }
            set
            {
                m_HaltOnEmpty = value;
            }
        }
        /// <summary>
        /// Gets or sets a boolean.  True if an Error status will prevent dialog completion.
        /// </summary>
        [Category("Behavior")]
        [Description("Get or Set.  True if an Error status will prevent dialog completion.")]
        public bool HaltOnError
        {
            get
            {
                return m_HaltOnError;
            }
            set
            {
                m_HaltOnError = value;
            }
        }
        /// <summary>
        /// Gets or sets the maximum allowable width for dynamic resize.  The dialog form sets this
        /// dynamically to the size of the container panel.  This is because the behavior for
        /// resizing larger than the size of the panel is erratic because of the autoscrolling
        /// behavior.
        /// </summary>
        [Category("Behavior")]
        [Description(" Sets the maximum allowable width for dynamic resize.  The dialog form sets this" +
            "dynamically to the size of the container panel.  This is because the behavior for resizing " +
            "larger than the size of the panel is erratic because of the autoscrolling behavior.")]
        public int MaxWidth
        {
            get
            {
                return m_MaxWidth;
            }
            set
            {
                m_MaxWidth = value;
            }
        }
        /// <summary>
        /// Gets or sets the minimum allowable width for dynamic resizing.  This specifically affects
        /// the behavior when the user uses the grip to resize the element.
        /// </summary>
        [Category("Behavior")]
        [Description("Gets or sets the minimum allowable width for dynamic resizing.  This specifically affects the behavior when the user uses the grip to resize the element.")]
        public int MinWidth
        {
            get
            {
                return m_MinWidth;
            }
            set
            {
                m_MinWidth = value;
            }
        }
        #endregion

        #region Properties - Help
        /// <summary>
        /// Gets or Sets the image that will appear after the help text in the panel, if any.
        /// </summary>
        [Category("Help")]
        [Description("Gets or Sets the image that will appear after the help text in the panel, if any.")]
        public Image HelpImage
        {
            get
            {
                return m_HelpImage;
            }
            set
            {
                m_HelpImage = value;
            }
        }

        /// <summary>
        /// Gets or Sets the text to appear in the help pannel.
        /// </summary>
        [Category("Help")]
        [Description("The text value that will populate the help pannel.")]
        public string HelpText
        {
            get
            {
                return m_HelpContent.ToString();
            }
            set
            {
                m_HelpContent = new StringBuilder();
                m_HelpContent.Append(value);
            }
        }
        /// <summary>
        /// Gets or Sets the title to be shown in the help panel.  Defaults to the Caption.
        /// </summary>
        [Category("Help")]
        [Description("Gets or Sets the title to be shown in the help panel.  Defaults to the Caption.")]
        public string HelpTitle
        {
            get
            {
                if (m_HelpTitle == null) return Caption;
                return m_HelpTitle;
            }
            set
            {
                m_HelpTitle = value;
            }
        }
        /// <summary>
        /// Gets or Sets the web address to use from the wiki button in the help panel.
        /// </summary>
        [Category("Help")]
        [Description("Gets or Sets the web address to use from the wiki button in the help panel.")]
        public string WikiAddress
        {
            get
            {
                return m_WikiAddress;
            }
            set
            {
                m_WikiAddress = value;
            }
        }
        /// <summary>
        /// Gets or Sets the default image that will be used in the help panel.
        /// </summary>
        [Category("Help")]
        [Description("Gets or Sets the default image that will be used in the help panel.")]
        public Image DefaultImage
        {
            get
            {
                return m_DefaultImage;
            }
            set
            {
                m_DefaultImage = value;
            }
        }
        /// <summary>
        /// Gets or Sets the alternate image that can be used in the help panel instead.
        /// </summary>
        [Category("Help")]
        [Description("Gets or Sets the alternate image that can be used in the help panel instead.")]
        public Image AlternateImage
        {
            get
            {
                return m_AlternateImage;
            }
            set
            {
                m_AlternateImage = value;
            }
        }
        #endregion


        /// <summary>
        /// Overrides the basic help button property to also handle the text box resize
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or Sets whether the help button is visible.")]
        public bool HelpButtonVisible
        {
            get
            {
                return m_HelpButtonVisible;
            }
            set
            {
                // The base function
                if (value == true && m_HelpButtonVisible == false)
                {
                    cbInput.Width -= 27;
                    m_HelpButtonVisible = true;
                    cmdHelp.Visible = true;
                    return;
                }

                if (value == false && m_HelpButtonVisible == true)
                {
                    cbInput.Width += 27;
                    m_HelpButtonVisible = false;
                    cmdHelp.Visible = false;
                    return;
                }

            }
        }
        /// <summary>
        /// Gets or sets whether a resize grip is provided for the user to change the dimensions of the control.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets whether or not the resize grip is visible.")]
        public bool ResizeGripVisible
        {
            get
            {
                return m_ResizeGripVisible;
            }
            set
            {
                if (value == true && m_ResizeGripVisible == false)
                {
                    cbInput.Width -= 15;
                    cmdHelp.Left -= 15;
                    lblGrip.Visible = true;
                    m_ResizeGripVisible = true;
                    return;
                }
                if (value == false && m_ResizeGripVisible == true)
                {
                    cbInput.Width += 15;
                    cmdHelp.Left += 15;
                    lblGrip.Visible = false;
                    m_ResizeGripVisible = false;
                    return;
                }

            }
        }
        #endregion

        #region ComboBox Element Specific Properties
        /// <summary>
        /// Gets or sets the list of layer names that will appear in the drop down
        /// </summary>
        /// 
        [Category("Appearance"),
        Description("Gets or sets the List Items.")]
        public string[] ListItems
        {
            get
            {
                return m_ListItems;
            }
            set
            {
                m_ListItems = value;
                cbInput.Items.Clear();
                for (int i = 0; i <= m_ListItems.GetUpperBound(0); i++)
                {
                    cbInput.Items.Add(m_ListItems[i]);
                }
            }
        }
        
        
        /// <summary>
        /// Gets or sets the value of the text in the text field.
        /// </summary>
        [Category("Data")]
        [Description("Gets and sets the value of the text in the combobox field.")]
        public string Value
        {
            get
            {
                return cbInput.Text;
            }
            set
            {
                if (m_ListItems != null)
                {
                    for (int i = 0; i <= m_ListItems.GetUpperBound(0); i++)
                    {
                        if (m_ListItems[i] == value)
                        {
                            cbInput.SelectedIndex = i;
                            LightMessage = "The item selected is valid.";
                            Status = LightStatus.Ok;
                            return;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Gets or sets the value of the text in the text field.
        /// This was added because the text property of the user control is confusing.
        /// </summary>
        [Category("Data")]
        [Description("Gets and sets the value of the text in the combobox field.")]
        public new string Text
        {
            get
            {
                return cbInput.Text;
            }
            set
            {
                if (m_ListItems != null)
                {
                    for (int i = 0; i <= m_ListItems.GetUpperBound(0); i++)
                    {
                        if (m_ListItems[i] == value)
                        {
                            cbInput.SelectedIndex = i;
                            LightMessage = "The item selected is valid.";
                            Status = LightStatus.Ok;
                            return;
                        }
                    }
                }
            }
        }

        #endregion

        #region ComboBox Element Specific Event Handling
        private void cbInput_Enter(object sender, System.EventArgs e)
        {
            if (cmdHelp.Visible == false)
            {
                // If our help button isn't visible, trigger help on enter
                // We don't want to force an opening unless the help pannel is visible
                OnHelpContextChanged(new HelpPannelEventArgs(this.HelpTitle, this.HelpText, this.HelpImage));
            }
        }

        private void cbInput_Validating(object sender, CancelEventArgs e)
        {
            if (cbInput.SelectedIndex >= 0)
            {
                LightMessage = "The item selected is valid.";
                Status = LightStatus.Ok;
                return;
            }
            else
            {
                LightMessage = "A valid item has not been selected.";
                Status = LightStatus.Empty;
                return;
            }
        }

        #endregion

        #region Event Handling


        private void cmdHelp_Click(object sender, EventArgs e)
        {
            HelpPannelEventArgs htxt = new HelpPannelEventArgs(HelpTitle, HelpText, HelpImage);
            OnHelpButtonPressed(htxt);
        }
        // When they "grab" the grip, it lets them resize this element
        private void lblGrip_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (m_AllowGripResize == false) return;
            // start keeping track of our mouse position
            m_Resizing = true;
            m_OldX = e.X;
            OnResizeStarted();

        }
        // Dragging while gripping resizes
        private void lblGrip_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_AllowGripResize == false) return;
            if (m_Resizing == true)
            {
                int NewWidth = this.Width + e.X - m_OldX;
                int GripSpacer = this.Width - lblGrip.Left;
                if (NewWidth > m_MaxWidth) NewWidth = m_MaxWidth;
                if (NewWidth < m_MinWidth) NewWidth = m_MinWidth;
                this.Width = NewWidth;
                lblGrip.Left = NewWidth - GripSpacer;
            }
        }

        private void lblGrip_MouseUp(object sender, MouseEventArgs e)
        {
            if (m_AllowGripResize == false) return;
            m_Resizing = false;
            OnResizeEnded();
        }
        void groupBox1_Click(object sender, System.EventArgs e)
        {
            OnClick(e);
        }

        #endregion
        

    }
    
}
