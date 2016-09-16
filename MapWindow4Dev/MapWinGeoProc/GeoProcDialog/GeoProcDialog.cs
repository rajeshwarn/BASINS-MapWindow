using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MapWinGeoProc.Dialogs
{
    /// <summary>
    /// A generic form that works with the various dialog elements in order to create 
    /// a fully working process.
    /// </summary>
    public partial class GeoProcDialog : Form
    {
        #region -------------- Variables
        int m_PanelWidth;
        // Help items when the user clicks on parts of the control other than a help activated element
        string m_HelpTitle;
        string m_HelpText;
        string m_HelpWebAddress;
        Image m_HelpImage;  // The background image
        Image l_HelpImage; // The actual image being displayed
        // The actual parameters that are changed as though they have values
        string l_HelpTitle;
        Font l_HelpTitleFont;
        string l_HelpText;
        Font l_HelpTextFont;
        bool m_WikiAreaVisible;
        bool m_HelpResizeButtonsVisible;

        /// <summary>
        /// A System.Collections.ArrayList that stores a list of all the DialogElements that have been added to this dialog
        /// </summary>
        public ArrayList Elements;
       // Point m_LockedPosition;
        /// <summary>
        /// Specifies the type of files the FileElemnt can use and whether or not it will be opening or saving them.
        /// </summary>
        public enum ElementTypes
        {
            /// <summary>
            /// The element will be used to open any file type.
            /// </summary>
            OpenFile,
            /// <summary>
            /// The element will be used to open grid files.
            /// </summary>
            OpenGridFile,
            /// <summary>
            /// The element will be used to open image files.
            /// </summary>
            OpenImageFile,
            /// <summary>
            /// The element will be used to open shapefiles.
            /// </summary>
            OpenShapefile,
            /// <summary>
            /// The element will be used to save any file type.
            /// </summary>
            SaveFile,
            /// <summary>
            /// The element will be used to save a grid file.
            /// </summary>
            SaveGridFile,
            /// <summary>
            /// The element will be used to save an image file.
            /// </summary>
            SaveImageFile,
            /// <summary>
            /// The element will be used to save a shapefile.
            /// </summary>
            SaveShapefile,

        }
        #endregion

        /// <summary>
        /// A class that will support a collection of DialogElements in order to 
        /// allow users to specify the parameters to be used for geoproc functions.
        /// </summary>
        public GeoProcDialog()
        {
            InitializeComponent();
            Elements = new ArrayList();
            m_PanelWidth = splitContainer1.Panel2.Width;
            splitContainer1.Panel2Collapsed = true;
            Width = Width - m_PanelWidth;
            cmdToggleHelp.Text = "Show Help >>";
            l_HelpTitleFont = new Font("Arial", 20, FontStyle.Bold);
            l_HelpTextFont = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
            l_HelpText = m_HelpText;
            l_HelpTitle = m_HelpTitle;
            m_WikiAreaVisible = true;
            m_HelpResizeButtonsVisible = true;
        }

        #region Properties
        /// <summary>
        /// A property to get the current help panel visible or sets it visible or not visible
        /// </summary>
        public bool HelpPanelVisible
        {
            get
            {
                return splitContainer1.Panel2Collapsed;
            }
            set
            {
                if (value == true && splitContainer1.Panel2Collapsed == true)
                {
                    m_PanelWidth = splitContainer1.Panel2.Width;
                    splitContainer1.Panel2Collapsed = true;
                    Width = Width - m_PanelWidth;
                    cmdToggleHelp.Text = "Show Help >>";
                }
                if(value == false && splitContainer1.Panel2Collapsed == false)
                {
                    Width = Width + m_PanelWidth;
                    splitContainer1.Panel2Collapsed = false;
                    cmdToggleHelp.Text = "<< Hide Help";
                }
            }
        }


        #region Button Visibility Options

        /// <summary>
        /// Boolean.  True if a wiki button with it's associated region will be shown at the top of the help panel.
        /// </summary>
        [Category("Appearance")]
        [Description("If true, a wiki button with it's associated region will be shown at the top of the help panel.")]
        public bool WikiAreaVisible
        {
            get
            {
                return m_WikiAreaVisible;
            }
            set
            {
                if (m_WikiAreaVisible == true) PanelWiki.Height = 0;
                else PanelWiki.Height = 32;
                m_WikiAreaVisible = value;
            }
        }
        /// <summary>
        /// Boolean.  If true, two buttons appear in the wiki area of the help panel to allow quick resizing.
        /// </summary>
        [Category("Appearance")]
        [Description("If true, two buttons appear in the wiki area of the help panel to allow quick resizing.")]
        public bool HelpResizeButtonsVisible
        {
            get
            {
                return m_HelpResizeButtonsVisible;
            }
            set
            {
                cmdEnlarge.Visible = value;
                cmdShrink.Visible = value;
                m_HelpResizeButtonsVisible = value;
            }
        }

        #endregion


        #region Properties - Help
        /// <summary>
        /// "Gets or Sets the help title that will appear for the entire dialog.  Defaults to the Dialog's Caption Text."
        /// </summary>
        [Category("Help"),
        Description("Gets or sets the help title that is actively shown in the help context.")]
        public string HelpTitle
        {
            get
            {
                if(l_HelpTitle == null)return this.Text;
                return l_HelpTitle;
            }
            set
            {
                l_HelpTitle = value;
            }
        }
        /// <summary>
        /// Gets or sets the default help title that appears initially, and when clicking outside of help enabled elements."
        /// </summary>
        [Category("Help")]
        [Description("Gets or sets the default help title that appears initially, and when clicking outside of help enabled elements.")]
        public string DialogHelpTitle
        {
            get
            {
                return m_HelpTitle;
            }
            set
            {
                m_HelpTitle = value;
                l_HelpTitle = value; // Setting the default also regenerates the default
                ChangeHelpContent();
            }
        }
        /// <summary>
        /// Gets or sets the font that will be used for any of the help titles shown in the help panel.
        /// </summary>
        [Category("Help")]
        [Description("Gets or sets the font that will be used for any of the help titles shown in the help panel.")]
        public Font HelpTitleFont
        {
            get
            {
                return l_HelpTitleFont;
            }
            set
            {
                l_HelpTitleFont = value;
            }
        }
        /// <summary>
        /// Gets or sets the help text that will appear for the entire dialog.
        /// </summary>
        [Category("Help")]
        [Description("Gets or sets the help text that will appear for the entire dialog.")]
        public string DialogHelpText
        {
            get
            {
                return m_HelpText;
            }
            set
            {
                m_HelpText = value;
                l_HelpText = value; // Setting this also forces the new value into the display
                ChangeHelpContent();
            }
        }
        /// <summary>
        /// Gets or sets the text that is currently being displayed in the help panel.
        /// </summary>
        [Category("Help")]
        [Description("Gets or sets the text that is currently being displayed in the help panel.")]
        public string HelpText
        {
            get
            {
                if(m_HelpText == null)return "No help text was given for this dialog.";
                return m_HelpText;
            }
            set
            {
                m_HelpText = value; // backup to use as a default
                l_HelpText = value; // the value that is actively being displayed
                ChangeHelpContent();
            }
        }
        /// <summary>
        /// Gets or sets the font that is used for the explanatory text in the help panel.
        /// </summary>
        [Category("Help")]
        [Description("Gets or sets the font that is used for the explanatory text in the help panel.")]
        public Font HelpTextFont
        {
            get
            {
                return l_HelpTextFont;
            }
            set
            {
                l_HelpTextFont = value;
            }
        }
        /// <summary>
        /// Gets or sets the backup help image that will appear initially and when clicking outside of help enabled elements.
        /// </summary>
        [Category("Help")]
        [Description("Gets or sets the backup help image that will appear initially and when clicking outside of help enabled elements.")]
        public Image DialogHelpImage
        {
            get
            {
                return m_HelpImage;
            }
            set
            {
                m_HelpImage = value;
                l_HelpImage = value; // Setting this also changes what is currently shown.
                if (value == null)
                {
                    lblHelpImage.Visible = false;
                }
                else
                {
                    lblHelpImage.Visible = true;
                }
                ChangeHelpContent();
            }
        }
        /// <summary>
        /// Gets or sets the image currently being shown in the help panel.
        /// </summary>
        public Image HelpImage
        {
            get
            {
                return l_HelpImage;
            }
            set
            {
                l_HelpImage = value;
            }

        }
        /// <summary>
        /// Gets or sets the URL that the wiki button will use for this dialog.
        /// </summary>
        [Category("Help"),
        Description("Gets or sets the URL that the wiki button will use for this dialog.")]
        public string HelpWebAddress
        {
            get
            {
                return m_HelpWebAddress;
            }
            set
            {
                m_HelpWebAddress = value;
            }
        }
        #endregion

        // Gets the values that were entered into each of the elements
        /// <summary>
        /// Gets the values that were entered into each of the elements.
        /// </summary>
        [Category("Data")]
        [Description("Gets the values that were entered into each of the elements.")]
        public ArrayList Parameters
        {
            get
            {
                ArrayList m_Parameters = new ArrayList();
                for (int I = 0; I < Elements.Count; I++)
                {
                    if (Elements[I].GetType() == typeof(FileElement))
                    {
                        m_Parameters.Add(((FileElement)Elements[I]).Filename);
                    }
                    if (Elements[I].GetType() == typeof(TextElement))
                    {
                        m_Parameters.Add(((TextElement)Elements[I]).Text);
                    }
                    if (Elements[I].GetType() == typeof(BooleanElement))
                    {
                        m_Parameters.Add(((BooleanElement)Elements[I]).Value);
                    }
                    if (Elements[I].GetType() == typeof(ComboBoxElement))
                    {
                        m_Parameters.Add(((ComboBoxElement)Elements[I]).Value);
                    }
                }
                return m_Parameters;
            }
        }
        /// <summary>
        /// Gets the captions for all the elements.
        /// In the case of boolean elements, this is the text next to the checkbox.
        /// </summary>
        [Category("Data")]
        [Description("Gets the captions for all the elements.")]
        public string[] Captions
        {
            get
            {
                string[] m_Captions = new string[Elements.Count];
                for (int I = 0; I < Elements.Count; I++)
                {
                    if (Elements[I].GetType() == typeof(BooleanElement))
                    {
                        m_Captions[I] = ((DialogElement)Elements[I]).Text;
                    }
                    else
                    {
                        m_Captions[I] = ((DialogElement)Elements[I]).Caption;
                    }
                }
                return m_Captions;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a an element to the dialog that allows the selection of a layer or a file.
        /// The layernames must be added manually.
        /// </summary>
        /// <param name="ElementType">Specifies exactly what sort of files will work</param>
        /// <returns>A LayerFileElement object that was added to the dialog</returns>
        public LayerFileElement Add_LayerFileElement(ElementTypes ElementType)
        {
            LayerFileElement MyLayerFileELement = new LayerFileElement();
            if (ElementType == ElementTypes.OpenFile || ElementType == ElementTypes.OpenGridFile ||
                ElementType == ElementTypes.OpenImageFile || ElementType == ElementTypes.OpenShapefile)
            {
                MyLayerFileELement.FileAccess = LayerFileElement.FileAccessType.Open;
                MyLayerFileELement.HelpImage = MyLayerFileELement.DefaultImage;
            }
            else
            {
                MyLayerFileELement.FileAccess = LayerFileElement.FileAccessType.Save;
                MyLayerFileELement.HelpImage = MyLayerFileELement.AlternateImage;
            }
            switch (ElementType)
            {
                case ElementTypes.OpenFile:
                    MyLayerFileELement.FileType = LayerFileElement.FileTypes.All;
                    MyLayerFileELement.Caption = "Input Filename";
                    MyLayerFileELement.HelpText = "Retrieves a filename for an unspecified file format either through the textbox or through the browse button.  A green light indicates that the file exists.  A red light indicates that the file does not exist.";
                    break;
                case ElementTypes.OpenGridFile:
                    MyLayerFileELement.FileType = LayerFileElement.FileTypes.Grid;
                    MyLayerFileELement.Caption = "Input Grid Filename";
                    MyLayerFileELement.HelpText = "Retrieves a filename for a grid file format either through the textbox or through the browse button.  A green light indicates that the file exists.  A red light indicates that the file does not exist.";
                    break;
                case ElementTypes.OpenImageFile:
                    MyLayerFileELement.FileType = LayerFileElement.FileTypes.Image;
                    MyLayerFileELement.Caption = "Input Image Filename";
                    MyLayerFileELement.HelpText = "Retrieves a filename for an image file format either through the textbox or through the browse button.  A green light indicates that the file exists.  A red light indicates that the file does not exist.";
                    break;
                case ElementTypes.OpenShapefile:
                    MyLayerFileELement.FileType = LayerFileElement.FileTypes.Shapefile;
                    MyLayerFileELement.Caption = "Input Shapefile Filename";
                    MyLayerFileELement.HelpText = "Retrieves a filename for a shapefile either through the textbox or through the browse button.  A green light indicates that the file exists.  A red light indicates that the file does not exist.";
                    break;
                case ElementTypes.SaveFile:
                    MyLayerFileELement.FileType = LayerFileElement.FileTypes.All;
                    MyLayerFileELement.Caption = "Output Filename";
                    MyLayerFileELement.HelpText = "Retrieves a filename for an unspecified file format either through the textbox or through the browse button.  A green light indicates that a file of that name does not exist yet or permission to overwrite was given through the browse dialog.  A red light indicates that a file with that name exists.";
                    break;
                case ElementTypes.SaveImageFile:
                    MyLayerFileELement.FileType = LayerFileElement.FileTypes.Image;
                    MyLayerFileELement.Caption = "Output Image Filename";
                    MyLayerFileELement.HelpText = "Retrieves a filename for an image file format either through the textbox or through the browse button.  A green light indicates that a file of that name does not exist yet or permission to overwrite was given through the browse dialog.  A red light indicates that a file with that name exists.";
                    break;
                case ElementTypes.SaveGridFile:
                    MyLayerFileELement.FileType = LayerFileElement.FileTypes.Grid;
                    MyLayerFileELement.Caption = "Output Grid Filename";
                    MyLayerFileELement.HelpText = "Retrieves a filename for a grid file format either through the textbox or through the browse button.  A green light indicates that a file of that name does not exist yet or permission to overwrite was given through the browse dialog.  A red light indicates that a file with that name exists.";
                    break;
                case ElementTypes.SaveShapefile:
                    MyLayerFileELement.FileType = LayerFileElement.FileTypes.Shapefile;
                    MyLayerFileELement.Caption = "Output Shapefile Filename";
                    MyLayerFileELement.HelpText = "Retrieves a filename for a shapefile either through the textbox or through the browse button.  A green light indicates that a file of that name does not exist yet or permission to overwrite was given through the browse dialog.  A red light indicates that a file with that name exists.";
                    break;
            }
            Elements.Add(MyLayerFileELement);
            MyLayerFileELement.ResizeGripVisible = false;
            MyLayerFileELement.HaltOnEmpty = true;
            MyLayerFileELement.HaltOnError = true;
            MyLayerFileELement.HelpButtonVisible = true;
            MyLayerFileELement.LightVisible = true;
            MyLayerFileELement.Location = new System.Drawing.Point(3, 10 + ((Elements.Count - 1) * 55));
            MyLayerFileELement.Name = "Element" + Elements.Count;
            int myWidth = this.panelElementContainer.Width - MyLayerFileELement.Left - 10;
            MyLayerFileELement.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top)));
            MyLayerFileELement.Size = new System.Drawing.Size(myWidth, 45);
            MyLayerFileELement.Status = LayerFileElement.LightStatus.Empty;
            MyLayerFileELement.HelpButtonPressed += new LayerFileElement.HelpButtonHandler(Element_HelpButtonPressed);
            MyLayerFileELement.ResizeStarted += new LayerFileElement.ResizeStartedHandler(Element_ResizeStarted);
            MyLayerFileELement.ResizeEnded += new LayerFileElement.ResizeEndedHandler(Element_ResizeEnded);
            MyLayerFileELement.HelpContextChanged += new LayerFileElement.HelpButtonHandler(MyFileElement_HelpContextChanged);
            MyLayerFileELement.Clicked += new EventHandler(panelElementContainer_Click);
            this.panelElementContainer.Controls.Add(MyLayerFileELement);
            return MyLayerFileELement;

        }

        /// <summary>
        /// Adds a new FileElement to the dialog and to the Elements arraylist.
        /// </summary>
        /// <param name="ElementType">An ElementTypes enumeration specifying what sort of files the project will use.</param>
        /// <returns></returns>
        public FileElement Add_FileElement(ElementTypes ElementType)
        {
            FileElement MyFileElement = new FileElement();
            if (ElementType == ElementTypes.OpenFile || ElementType == ElementTypes.OpenGridFile || 
                ElementType == ElementTypes.OpenImageFile || ElementType == ElementTypes.OpenShapefile)
            {
                MyFileElement.FileAccess = FileElement.FileAccessType.Open;
                MyFileElement.HelpImage = MyFileElement.DefaultImage;
            }
            else
            {
                MyFileElement.FileAccess = FileElement.FileAccessType.Save;
                MyFileElement.HelpImage = MyFileElement.AlternateImage;
            }
            switch (ElementType)
            {
                case ElementTypes.OpenFile:
                    MyFileElement.FileType = FileElement.FileTypes.All;
                    MyFileElement.Caption = "Input Filename";
                    MyFileElement.HelpText = "Retrieves a filename for an unspecified file format either through the textbox or through the browse button.  A green light indicates that the file exists.  A red light indicates that the file does not exist.";
                    break;
                case ElementTypes.OpenGridFile:
                    MyFileElement.FileType = FileElement.FileTypes.Grid;
                    MyFileElement.Caption = "Input Grid Filename";
                    MyFileElement.HelpText = "Retrieves a filename for a grid file format either through the textbox or through the browse button.  A green light indicates that the file exists.  A red light indicates that the file does not exist.";
                    break;
                case ElementTypes.OpenImageFile:
                    MyFileElement.FileType = FileElement.FileTypes.Image;
                    MyFileElement.Caption = "Input Image Filename";
                    MyFileElement.HelpText = "Retrieves a filename for an image file format either through the textbox or through the browse button.  A green light indicates that the file exists.  A red light indicates that the file does not exist.";
                    break;
                case ElementTypes.OpenShapefile:
                    MyFileElement.FileType = FileElement.FileTypes.Shapefile;
                    MyFileElement.Caption = "Input Shapefile Filename";
                    MyFileElement.HelpText = "Retrieves a filename for a shapefile either through the textbox or through the browse button.  A green light indicates that the file exists.  A red light indicates that the file does not exist.";
                    break;
                case ElementTypes.SaveFile:
                    MyFileElement.FileType = FileElement.FileTypes.All;
                    MyFileElement.Caption = "Output Filename";
                    MyFileElement.HelpText = "Retrieves a filename for an unspecified file format either through the textbox or through the browse button.  A green light indicates that a file of that name does not exist yet or permission to overwrite was given through the browse dialog.  A red light indicates that a file with that name exists.";
                    break;
                case ElementTypes.SaveImageFile:
                    MyFileElement.FileType = FileElement.FileTypes.Image;
                    MyFileElement.Caption = "Output Image Filename";
                    MyFileElement.HelpText = "Retrieves a filename for an image file format either through the textbox or through the browse button.  A green light indicates that a file of that name does not exist yet or permission to overwrite was given through the browse dialog.  A red light indicates that a file with that name exists.";
                    break;
                case ElementTypes.SaveGridFile:
                    MyFileElement.FileType = FileElement.FileTypes.Grid;
                    MyFileElement.Caption = "Output Grid Filename";
                    MyFileElement.HelpText = "Retrieves a filename for a grid file format either through the textbox or through the browse button.  A green light indicates that a file of that name does not exist yet or permission to overwrite was given through the browse dialog.  A red light indicates that a file with that name exists.";
                    break;
                case ElementTypes.SaveShapefile:
                    MyFileElement.FileType = FileElement.FileTypes.Shapefile;
                    MyFileElement.Caption = "Output Shapefile Filename";
                    MyFileElement.HelpText = "Retrieves a filename for a shapefile either through the textbox or through the browse button.  A green light indicates that a file of that name does not exist yet or permission to overwrite was given through the browse dialog.  A red light indicates that a file with that name exists.";
                    break;
            }
            Elements.Add(MyFileElement);
            MyFileElement.ResizeGripVisible = false;
            MyFileElement.HaltOnEmpty = true;
            MyFileElement.HaltOnError = true;
            MyFileElement.HelpButtonVisible = true;
            MyFileElement.LightVisible = true;
            MyFileElement.Location = new System.Drawing.Point(3, 10 + ((Elements.Count - 1) * 55));
            MyFileElement.Name = "Element" + Elements.Count;
            int myWidth = this.panelElementContainer.Width - MyFileElement.Left - 10;
            MyFileElement.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top)));
            MyFileElement.Size = new System.Drawing.Size(myWidth, 45);
            MyFileElement.Status = FileElement.LightStatus.Empty;
            MyFileElement.HelpButtonPressed += new FileElement.HelpButtonHandler(Element_HelpButtonPressed);
            MyFileElement.ResizeStarted += new FileElement.ResizeStartedHandler(Element_ResizeStarted);
            MyFileElement.ResizeEnded += new FileElement.ResizeEndedHandler(Element_ResizeEnded);
            MyFileElement.HelpContextChanged += new FileElement.HelpButtonHandler(MyFileElement_HelpContextChanged);
            MyFileElement.Clicked += new EventHandler(panelElementContainer_Click);
            this.panelElementContainer.Controls.Add(MyFileElement);
            return MyFileElement;
        }

        /// <summary>
        /// Adds a new ComboBoxElement to the dialog and to the Elements arraylist.
        /// </summary>
        /// <returns></returns>
        public ComboBoxElement Add_ComboBoxElement()
        {
            ComboBoxElement MyComboboxElement = new ComboBoxElement();
            Elements.Add(MyComboboxElement);
            this.panelElementContainer.Controls.Add(MyComboboxElement);
            Initialize_Element(MyComboboxElement);
            return MyComboboxElement;
        }
       
        /// <summary>
        /// Adds a new TextElement control to the dialog to receive a text parameter from a user.
        /// </summary>
        /// <returns></returns>
        public TextElement Add_TextElement()
        {
            TextElement MyTextElement = new TextElement();
            Elements.Add(MyTextElement);
            this.panelElementContainer.Controls.Add(MyTextElement);
            Initialize_Element(MyTextElement);
            return MyTextElement;
        }
        /// <summary>
        /// Adds a new BooleanElement control to the dialog to receive true false information from
        /// the users.
        /// </summary>
        /// <returns></returns>
        public BooleanElement Add_BooleanElement()
        {
            BooleanElement MyBooleanElement = new BooleanElement();
            Elements.Add(MyBooleanElement);
            this.panelElementContainer.Controls.Add(MyBooleanElement);
            Initialize_Element(MyBooleanElement);
           
            return MyBooleanElement;
        }

       
       
     
     

        #endregion

        #region Event Handling

        #region ----------- Help Area Events

        // Someone pressed the wiki button that takes you to a hyperlink
        private void cmdWiki_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            if (m_HelpWebAddress == null)
            {
                process.StartInfo.FileName = "http://www.mapwindow.org/wiki/index.php?title=MapWinGeoProc_Developer%27s_Guide";
            }
            else
            {
                process.StartInfo.FileName = m_HelpWebAddress;
            }
            process.Start();
        }

        // Mouse wheel over help panel
        void HelpPanel_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {

            if (splitContainer1.Panel2.VerticalScroll.Visible == false) return;
            int val = splitContainer1.Panel2.VerticalScroll.Value;
            val -= e.Delta;
            if (val < splitContainer1.Panel2.VerticalScroll.Minimum) val = this.splitContainer1.Panel2.VerticalScroll.Minimum;
            if (val > splitContainer1.Panel2.VerticalScroll.Maximum) val = this.splitContainer1.Panel2.VerticalScroll.Maximum;
            this.splitContainer1.Panel2.VerticalScroll.Value = val;
        }

        // This just ensures that we can scroll if we click in the help area
        void PanelWiki_Click(object sender, System.EventArgs e)
        {
            this.splitContainer1.Panel2.Focus();
        }

        // This button easily enlarges and centers the form
        private void cmdEnlarge_Click(object sender, EventArgs e)
        {
            // increases the size of the control, hopefully enough for help to be visible

            this.Width += 250;
            this.Height += 250;
            this.CenterToScreen();
            ResizeHelp();

        }

        // This button easily shrinks and centers the form
        private void cmdShrink_Click(object sender, EventArgs e)
        {
            this.Width -= 250;
            this.Height -= 250;
            this.CenterToScreen();
            ResizeHelp();
        }

        #endregion

        #region ----------- Element Area Events

        // Reset the default help content when the user clicks outside the sensitive area
        // also give focus to the container to recieve vertical scroll events
        void panelElementContainer_Click(object sender, System.EventArgs e)
        {
            panelElementContainer.Focus();
            if (splitContainer1.Panel2Collapsed == true) return;

            l_HelpTitle = this.HelpTitle;
            l_HelpText = this.HelpText;
            lblHelpImage.Image = this.HelpImage;
            ChangeHelpContent();
            ResizeHelp();

        }

        // Mouse wheel over element container panel
        void Panel1_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (this.panelElementContainer.VerticalScroll.Visible == false) return;
            int val = this.panelElementContainer.VerticalScroll.Value;
            val -= e.Delta;
            if (val < this.panelElementContainer.VerticalScroll.Minimum) val = this.panelElementContainer.VerticalScroll.Minimum;
            if (val > this.panelElementContainer.VerticalScroll.Maximum) val = this.panelElementContainer.VerticalScroll.Maximum;
            this.panelElementContainer.VerticalScroll.Value = val;
        }

        
        // Someone pressed a help button, so open the help for that context and expand the help panel
        private void Element_HelpButtonPressed(object sender, HelpPannelEventArgs e)
        {
            if (splitContainer1.Panel2Collapsed == true)
            {
                Width = Width + m_PanelWidth;
                splitContainer1.Panel2Collapsed = false;
                cmdToggleHelp.Text = "<< Hide Help";
            }
            l_HelpTitle = e.Title;
            l_HelpText = e.Text;
            lblHelpImage.Image = e.Image;
            ChangeHelpContent();
            ResizeHelp();
        }
        // Someone entered a help sensitive area, so if the help is already open, add new content
        void MyFileElement_HelpContextChanged(object sender, HelpPannelEventArgs e)
        {
            // Change the help content only if the panel is already open
            if (splitContainer1.Panel2Collapsed == false)
            {
                l_HelpTitle = e.Title;
                l_HelpText = e.Text;
                l_HelpImage = e.Image;
                ChangeHelpContent();
                ResizeHelp();
            }
        }

        // The button that collapses or expands the help panel was pressed
        private void cmdToggleHelp_Click(object sender, EventArgs e)
        {

            if (splitContainer1.Panel2Collapsed == false)
            {
                m_PanelWidth = splitContainer1.Panel2.Width;
                splitContainer1.Panel2Collapsed = true;
                Width = Width - m_PanelWidth;
                cmdToggleHelp.Text = "Show Help >>";
            }
            else
            {
                Width = Width + m_PanelWidth;
                splitContainer1.Panel2Collapsed = false;
                cmdToggleHelp.Text = "<< Hide Help";
            }
        }

        void Element_ResizeStarted(object sender)
        {

            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            ((DialogElement)sender).MaxWidth = splitContainer1.Panel1.Width - ((DialogElement)sender).Left;

        }

        void Element_ResizeEnded(object sender)
        {
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            //m_LockedPosition = null; //this.panelElementContainer.AutoScroll = true;
        }

        #endregion
       
       
        void frmCustomDialog_Resize(object sender, System.EventArgs e)
        {
            // When we resize this form, we have to dynamically format our help text and image placement
           ResizeHelp();
        }
        void frmCustomDialog_Shown(object sender, System.EventArgs e)
        {
            
            // When showing this form, we have to dynamically format our help text and image placement.
            if (splitContainer1.Panel2Collapsed == true) return;
            l_HelpTitle = DialogHelpTitle;
            l_HelpText = DialogHelpText;
            l_HelpImage = DialogHelpImage;
            ChangeHelpContent();
            ResizeHelp();
            //throw new System.Exception("The method or operation is not implemented.");
        }
        
        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
           ResizeHelp();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
           
            for (int I = 0; I < Elements.Count; I++)
            {
                Type elType = Elements[I].GetType();
                if (elType == typeof(FileElement))
                {
                    FileElement fe = Elements[I] as FileElement;
                    if (fe.Halt == true)
                    {
                        string caption = fe.Caption;
                        string text;
                        text = "The value you entered for " + caption + " has the following error:\n" +
                        fe.LightMessage + "\n";
                        MessageBox.Show(text, "Some Parameters are invalid", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
                if (elType == typeof(LayerFileElement))
                {
                    LayerFileElement fe = Elements[I] as LayerFileElement;
                    if (fe.Halt == true)
                    {
                        string caption = fe.Caption;
                        string text;
                        text = "The value you entered for " + caption + " has the following error:\n" +
                        fe.LightMessage + "\n";
                        MessageBox.Show(text, "Some Parameters are invalid", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
                else if (elType == typeof(TextElement))
                {
                    TextElement fe = Elements[I] as TextElement;
                    if (fe.Halt == true)
                    {
                        string caption = fe.Caption;
                        string text;
                        text = "The value you entered for " + caption + " has the following error:\n" +
                        fe.LightMessage + "\n";
                        MessageBox.Show(text, "Some Parameters are invalid", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                }
                else if (elType == typeof(ComboBoxElement))
                {
                    ComboBoxElement fe = Elements[I] as ComboBoxElement;
                    if (fe.Halt == true)
                    {
                        string caption = fe.Caption;
                        string text;
                        text = "The value you entered for " + caption + " has the following error:\n" +
                        fe.LightMessage + "\n";
                        MessageBox.Show(text, "Some Parameters are invalid", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }               
            }
            this.DialogResult = DialogResult.OK;
            if (!this.Modal)
            {
                this.Hide();
            }
        }

        private void cmdCancle_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

            if (!this.Modal)
            {
                this.Hide();
            }
        }
        
       
        #endregion

        #region Private Functions
        void ChangeHelpContent()
        {      
            rtbHelpTitle.Clear();
            if (l_HelpTitle != null)
            {
                rtbHelpTitle.SelectedText = l_HelpTitle + "\n\n";
                rtbHelpTitle.SelectionFont = l_HelpTitleFont;
            }
            if (l_HelpText != null)
            {
                rtbHelpTitle.SelectionStart = rtbHelpTitle.Text.Length;
                rtbHelpTitle.SelectionFont = l_HelpTextFont;
                rtbHelpTitle.SelectedText = l_HelpText;
            }
            if (l_HelpImage != null)
            {
                lblHelpImage.Image = l_HelpImage;
            }
        }
        void ResizeHelp()
        {
            if (l_HelpTitleFont == null) return;
            if (this.Width + 250 < Screen.GetWorkingArea(this).Width) cmdEnlarge.Enabled = true;
            if (this.Height + 250 < Screen.GetWorkingArea(this).Height) cmdEnlarge.Enabled = true;
            if (this.Width - 250 > this.panelElementContainer.Width) cmdShrink.Enabled = true;
            if (this.Height - 250 > 0) cmdShrink.Enabled = true;
            if (this.Width - 250 < this.panelElementContainer.Width) cmdShrink.Enabled = false;
            if (this.Height - 250 < 0) cmdShrink.Enabled = false;
            if (this.Width + 250 > Screen.GetWorkingArea(this).Width) cmdEnlarge.Enabled = false;
            if (this.Height + 250 > Screen.GetWorkingArea(this).Height) cmdEnlarge.Enabled = false;
            // First resize our title textbox to allow wrapping
            Graphics g = rtbHelpTitle.CreateGraphics();
            SizeF TitleSize = g.MeasureString(l_HelpTitle+"\n\n", l_HelpTitleFont, rtbHelpTitle.Width);
           

            g = rtbHelpTitle.CreateGraphics();
            SizeF TextSize = g.MeasureString(l_HelpText, l_HelpTextFont, rtbHelpTitle.Width);
            
            panelHelpTitle.Height = (int)TitleSize.Height + (int)TextSize.Height + 5;
            
            if (l_HelpImage == null) return;
            
            lblHelpImage.Top = panelHelpTitle.Top + panelHelpTitle.Height + 5;
            lblHelpImage.Width = l_HelpImage.Width;
            lblHelpImage.Height = l_HelpImage.Height;
           
            
        }

        void Initialize_Element(DialogElement MyElement)
        {
            MyElement.Location = new System.Drawing.Point(3, 10 + ((Elements.Count - 1) * 55));
            MyElement.Name = "Element" + Elements.Count;
            MyElement.Size = new System.Drawing.Size(457, 45);
            MyElement.Status = DialogElement.LightStatus.Empty;
            MyElement.ResizeStarted += new DialogElement.ResizeStartedHandler(Element_ResizeStarted);
            MyElement.ResizeEnded += new DialogElement.ResizeEndedHandler(Element_ResizeEnded);
            MyElement.HelpButtonPressed += new DialogElement.HelpButtonHandler(Element_HelpButtonPressed);
            MyElement.HelpImage = MyElement.DefaultImage;
            int myWidth = this.panelElementContainer.Width - MyElement.Left - 10;
            MyElement.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top)));
            MyElement.Size = new System.Drawing.Size(myWidth, 45);
            MyElement.ResizeGripVisible = false;
            MyElement.Clicked += new EventHandler(panelElementContainer_Click);
        }
        void Initialize_Element(BooleanElement MyElement)
        {
            MyElement.Location = new System.Drawing.Point(3, 10 + ((Elements.Count - 1) * 55));
            MyElement.Name = "Element" + Elements.Count;
            MyElement.Size = new System.Drawing.Size(457, 45);
            MyElement.ResizeStarted += new BooleanElement.ResizeStartedHandler(Element_ResizeStarted);
            MyElement.ResizeEnded += new BooleanElement.ResizeEndedHandler(Element_ResizeEnded);
            MyElement.HelpButtonPressed += new BooleanElement.HelpButtonHandler(Element_HelpButtonPressed);
            MyElement.HelpImage = MyElement.DefaultImage;
            int myWidth = this.panelElementContainer.Width - MyElement.Left - 10;
            MyElement.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top)));
            MyElement.Size = new System.Drawing.Size(myWidth, 45);
            MyElement.ResizeGripVisible = false;
            MyElement.Clicked += new EventHandler(panelElementContainer_Click);
        }
        void Initialize_Element(TextElement MyElement)
        {
            MyElement.Location = new System.Drawing.Point(3, 10 + ((Elements.Count - 1) * 55));
            MyElement.Name = "Element" + Elements.Count;
            MyElement.Size = new System.Drawing.Size(457, 45);
            MyElement.Status = TextElement.LightStatus.Empty;
            MyElement.ResizeStarted += new TextElement.ResizeStartedHandler(Element_ResizeStarted);
            MyElement.ResizeEnded += new TextElement.ResizeEndedHandler(Element_ResizeEnded);
            MyElement.HelpButtonPressed += new TextElement.HelpButtonHandler(Element_HelpButtonPressed);
            MyElement.HelpImage = MyElement.DefaultImage;
            int myWidth = this.panelElementContainer.Width - MyElement.Left - 10;
            MyElement.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top)));
            MyElement.Size = new System.Drawing.Size(myWidth, 45);
            MyElement.ResizeGripVisible = false;
            MyElement.Clicked += new EventHandler(panelElementContainer_Click);
        }
        void Initialize_Element(ComboBoxElement MyElement)
        {
            MyElement.Location = new System.Drawing.Point(3, 10 + ((Elements.Count - 1) * 55));
            MyElement.Name = "Element" + Elements.Count;
            MyElement.Size = new System.Drawing.Size(457, 45);
            MyElement.Status = ComboBoxElement.LightStatus.Empty;
            MyElement.ResizeStarted += new ComboBoxElement.ResizeStartedHandler(Element_ResizeStarted);
            MyElement.ResizeEnded += new ComboBoxElement.ResizeEndedHandler(Element_ResizeEnded);
            MyElement.HelpButtonPressed += new ComboBoxElement.HelpButtonHandler(Element_HelpButtonPressed);
            MyElement.HelpImage = MyElement.DefaultImage;
            int myWidth = this.panelElementContainer.Width - MyElement.Left - 10;
            MyElement.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top)));
            MyElement.Size = new System.Drawing.Size(myWidth, 45);
            MyElement.ResizeGripVisible = false;
            MyElement.Clicked += new EventHandler(panelElementContainer_Click);
        }

        #endregion

        

       
        
        

       

       
        


    }
}