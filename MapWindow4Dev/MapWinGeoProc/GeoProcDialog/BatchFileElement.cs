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
    /// A modular control that can be used for specifying a filename for opening or closing files.
    /// </summary>
    public partial class BatchFileElement : UserControl
    {
        #region Variables

        #region Generic

        LightStatus m_Status;
        bool m_HaltOnError;
        bool m_HaltOnEmpty;
        StringBuilder m_HelpContent; // Stores help content to be displayed
        Image m_HelpImage;

        //Resizing 
        bool m_ResizeGripVisible;
        bool m_AllowGripResize;
       
        int m_OldX;
        int m_MaxWidth;
        int m_MinWidth;

        // Help Stuff
        bool m_HelpButtonVisible; // necessary for changing this before the button exists
        string m_HelpTitle;
        string m_WikiAddress;
        Image m_DefaultImage;
        Image m_AlternateImage;

        // Batch file handling
        DataTable tblFiles;
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


        string m_Filter;
        FileTypes m_FileType;
        FileAccessType m_FileAccess;




        /// <summary>
        /// Specifies a GIS file category to help narrow down the open/save file dialogs
        /// and improve the file format validation.
        /// </summary>
        public enum FileTypes
        {
            /// <summary>
            /// Shapefiles that end with extension .shp
            /// </summary>
            Shapefile,
            /// <summary>
            /// Grid file formats like geotif, asc, esri grids etc.
            /// </summary>
            Grid,
            /// <summary>
            /// Image file formats like bmp, jpg, gif, etc.
            /// </summary>
            Image,
            /// <summary>
            /// The element will not use any filter for file extensions or validation.
            /// </summary>
            All
        }
        /// <summary>
        /// Specifies whether an element is being used to open or save a file.
        /// </summary>
        public enum FileAccessType
        {
            /// <summary>
            /// Speficies that the element will be used to open files.
            /// </summary>
            Open,
            /// <summary>
            /// Specifies that the element will be used to save files.
            /// </summary>
            Save
        }
        #endregion

        /// <summary>
        /// Creates a new instance of the FileElement class
        /// </summary>
        public BatchFileElement()
        {
           
            InitializeComponent();
            ttBrowse.SetToolTip(cmdBrowse, "Browse.");
            DataColumn dcFilename = new DataColumn("Filename", typeof(string));
            tblFiles = new DataTable();
            tblFiles.Columns.Add(dcFilename);
            dataGridView1.DataSource = tblFiles;
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

        #region General

        

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


        #endregion

        /// <summary>
        /// Gets or sets whether the file is opened or saved
        /// </summary>
        [Category("Behavior")]
        [Description("Determines whether this file dialog is used for opening existing files or saving new files.")]
        public FileAccessType FileAccess
        {
            get
            {
                return m_FileAccess;
            }
            set
            {
                m_FileAccess = value;
            }
        }
        /// <summary>
        /// Gets or sets the filename shown in the textbox for this element
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the filename shown in the textbox for this element.")]
        public string Filename
        {
            get
            {
                return tbFilename.Text;
            }
            set
            {
                tbFilename.Text = value;
                if (m_FileAccess == FileAccessType.Open)
                {
                    if (System.IO.File.Exists(value))
                    {
                        this.Status = LightStatus.Ok;
                        LightMessage = "The filename specified is valid.";
                    }
                    else
                    {
                        this.Status = LightStatus.Error;
                        LightMessage = "The specified file cannot be found.";
                    }
                }
                else
                {
                    if (System.IO.File.Exists(value))
                    {
                        this.Status = LightStatus.Error;
                        LightMessage = "A file by the specified name already exists.";
                    }
                    else
                    {
                        this.Status = LightStatus.Ok;
                        LightMessage = "The filename specified is valid.";
                    }
                }
            }

        }

        /// <summary>
        /// Gets or Sets the allowable file types by specifying shapefiles, images, or grids.
        /// </summary>
        [Category("Behavior"),
        Description("Gets or Sets the allowable file types by specifying shapefiles, images, or grids.")]
        public FileTypes FileType
        {
            get
            {
                return m_FileType;
            }
            set
            {
                m_FileType = value;
                if (m_FileType == FileTypes.Shapefile)
                {

                    m_Filter = "Shapefiles (*.shp)|*.shp";
                    return;
                }
                if (m_FileType == FileTypes.Image)
                {
                    if (m_FileAccess == FileAccessType.Open)
                    {
                        m_Filter = "All Supported Image Types|hdr.adf;*.asc;*.bt;*.bil;*.bmp;*.ecw;*.img;*.gif;*.map;*.jp2;*.jpg;*.sid;*.pgm;*.pnm;*.png;*.ppm;*.tif|ArcInfo Grid Images (hdr.adf)|hdr.adf|ASCII Grid Images (*.asc)|*.asc|Binary Terrain Images (*.bt)|*.bt|BIL (ESRI HDR/BIL Images) (*.bil)|*.bil|Bitmap Images (*.bmp)|*.bmp|ECW (Enhanced Compression Wavelet) (*.ecw)|*.ecw|Erdas Imagine Images (*.img)|*.img|GIF Images (*.gif)|*.gif|PC Raster Images (*.map)|*.map|JPEG2000 Images (*.jp2)|*.jp2|JPEG Images (*.jpg)|*.jpg|SID (MrSID Images) (*.sid)|*.sid|Portable Network Images (*.pgm,*.pnm,*.png,*.ppm)|*.pgm;*.pnm;*.png;*.ppm|Tagged Image File Format (*.tif)|*.tif";
                    }
                    else
                    {
                        m_Filter = "All Supported Image Types|*.asc;*.bt;*.bil;*.bmp;*.ecw;*.img;*.gif;*.map;*.jp2;*.jpg;*.sid;*.pgm;*.pnm;*.png;*.ppm;*.tif|ASCII Grid Images (*.asc)|*.asc|Binary Terrain Images (*.bt)|*.bt|BIL (ESRI HDR/BIL Images) (*.bil)|*.bil|Bitmap Images (*.bmp)|*.bmp|ECW (Enhanced Compression Wavelet) (*.ecw)|*.ecw|Erdas Imagine Images (*.img)|*.img|GIF Images (*.gif)|*.gif|PC Raster Images (*.map)|*.map|JPEG2000 Images (*.jp2)|*.jp2|JPEG Images (*.jpg)|*.jpg|SID (MrSID Images) (*.sid)|*.sid|Portable Network Images (*.pgm,*.pnm,*.png,*.ppm)|*.pgm;*.pnm;*.png;*.ppm|Tagged Image File Format (*.tif)|*.tif";
                    }
                    return;
                }
                if (m_FileType == FileTypes.Grid)
                {
                    if (m_FileAccess == FileAccessType.Open)
                    {
                        m_Filter = "All Supported Grid Formats|sta.adf;*.bgd;*.asc;*.tif;????cel0.ddf;*.arc;*.aux;*.pix;*.dhm;*.dt0;*.dt1;*.bil;|USU Binary (*.bgd)|*.bgd|BIL (ESRI HDR/BIL Images) (*.bil)|*.bil|ASCII Text (ESRI Ascii Grid) (*.asc, *.arc)|*.asc;*.arc|GeoTIFF (*.tif)|*.tif|ESRI Grid|sta.adf|ESRI FLT|*.flt|USGS SDTS 30m (*.ddf)|????cel0.ddf|PAux (PCI .aux Labelled) (*.aux)|*.aux|PIX (PCIDSK Database File) (*.pix)|*.pix|DTED (DTED Elevation Raster) (*.dhm, *.dt0, *.dt1)|*.dhm;*.dt0;*.dt1";
                    }
                    else
                    {
                        m_Filter = "All Supported Grid Formats|*.bgd;*.asc;*.tif;????cel0.ddf;*.arc;*.aux;*.pix;*.dhm;*.dt0;*.dt1;*.bil;|USU Binary (*.bgd)|*.bgd|BIL (ESRI HDR/BIL Images) (*.bil)|*.bil|ASCII Text (ESRI Ascii Grid) (*.asc, *.arc)|*.asc;*.arc|GeoTIFF (*.tif)|*.tif|USGS SDTS 30m (*.ddf)|????cel0.ddf|PAux (PCI .aux Labelled) (*.aux)|*.aux|PIX (PCIDSK Database File) (*.pix)|*.pix|DTED (DTED Elevation Raster) (*.dhm, *.dt0, *.dt1)|*.dhm;*.dt0;*.dt1";
                    }

                    return;
                }
                m_Filter = string.Empty;
            }
        }

        /// <summary>
        /// Gets or Sets the dialog filter to be used directly.  Setting AllowedFileTypes will
        /// automatically set this with the filter values allowed for grids, images or shapefiles.
        /// </summary>
        [Category("Behavior")]
        [Description("Gets or Sets the dialog filter to be used directly.")]
        public string Filter
        {
            get
            {
                return m_Filter;
            }
            set
            {
                m_Filter = value;
            }
        }

        /// <summary>
        /// Override the basic help button property change to also handle the text box resize
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
                // In addition to the normal parameter, also control the textbox and browse elements
                if (value == true && m_HelpButtonVisible == false)
                {
                    tbFilename.Width -= 27;
                    cmdBrowse.Left -= 27;
                    m_HelpButtonVisible = true;
                    cmdHelp.Visible = true;
                    return;
                }

                if (value == false && m_HelpButtonVisible == true)
                {
                    tbFilename.Width += 27;
                    cmdBrowse.Left += 27;
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
                    tbFilename.Width -= 15;
                    cmdBrowse.Left -= 15;
                    cmdHelp.Left -= 15;
                    m_ResizeGripVisible = value;
                    return;
                }
                if (value == false && m_ResizeGripVisible == true)
                {
                    tbFilename.Width += 15;
                    cmdBrowse.Left += 15;
                    cmdHelp.Left += 15;
                    m_ResizeGripVisible = value;
                    return;
                }
            }
        }
        #endregion


        #region Methods
        /// <summary>
        /// Appends a string to the end of the help content
        /// </summary>
        /// <param name="HelpText"></param>
        public void HelpText_Append(string HelpText)
        {
            m_HelpContent.Append(HelpText);
        }
        /// <summary>
        /// Clears the existing help content.
        /// </summary>
        /// <param name="HelpText"></param>
        public void HelpText_Clear(string HelpText)
        {
            m_HelpContent = new StringBuilder();
        }
        #endregion

        #region Event Handling

        #region general

      


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
            
            m_OldX = e.X;
            OnResizeStarted();

        }
       
        private void lblGrip_MouseUp(object sender, MouseEventArgs e)
        {
            if (m_AllowGripResize == false) return;
            
            OnResizeEnded();
        }

       
        #endregion


        private void cmdBrowse_Click(object sender, EventArgs e)
        {
            
            if (m_FileAccess == FileAccessType.Open)
            {
                OpenFileDialog FD = new OpenFileDialog();
                FD.Filter = m_Filter;
                FD.Multiselect = true;
                if (FD.ShowDialog() == DialogResult.OK)
                {
                    tbFilename.Text = FD.FileName;
                    if (FD.FileNames == null)
                    {
                        DataRow dr = tblFiles.NewRow();
                        dr["Filename"] = FD.FileName;
                        tblFiles.Rows.Add(dr);

                    }
                    if (FD.FileNames != null)
                    {
                        for (int I = 0; I <= FD.FileNames.GetUpperBound(0); I++)
                        {
                            DataRow dr = tblFiles.NewRow();
                            dr["Filename"] = FD.FileNames[I];
                            tblFiles.Rows.Add(dr);
                        }
                    }
                    this.Status = LightStatus.Ok;
                    LightMessage = "The filename specified is valid.";
                }
                else
                {
                    LightMessage = "A filename has not been specified.";
                    Status = LightStatus.Empty;
                    tbFilename.Text = "";
                }
            }
            else
            {
                SaveFileDialog FD = new SaveFileDialog();
                FD.Filter = m_Filter;
                if (FD.ShowDialog() == DialogResult.OK)
                {
                    tbFilename.Text = FD.FileName;
                    this.Status = LightStatus.Ok;
                    if (System.IO.File.Exists(tbFilename.Text))
                    {
                        LightMessage = "The specified file will be overwritten.";
                    }
                    else
                    {
                        LightMessage = "The filename specified is valid.";
                    }
                }
                else
                {
                    tbFilename.Text = "";
                    this.Status = LightStatus.Empty;
                    LightMessage = "A filename has not been specified.";
                }
            }
            
           
        }

        void tbFilename_Enter(object sender, System.EventArgs e)
        {
            if (cmdHelp.Visible == false)
            {
                // If our help button isn't visible, trigger help on enter
                // We don't want to force an opening unless the help pannel is visible
                OnHelpContextChanged(new HelpPannelEventArgs(this.HelpTitle, this.HelpText, this.HelpImage));
            }
        }

        private void tbFilename_Validating(object sender, EventArgs e)
        {
            if (tbFilename.Text == string.Empty)
            {
                LightMessage = "A filename has not been specified.";
                Status = LightStatus.Empty;
                return;
            }
            if(System.IO.File.Exists(tbFilename.Text))
            {
                if (m_FileAccess == FileAccessType.Open)
                {
                    string ext = System.IO.Path.GetExtension(tbFilename.Text);
                    if (IsValidExtension(ext))
                    {
                        this.LightMessage = "The filename specified is valid.";
                        Status = LightStatus.Ok;
                    }
                    else
                    {
                        this.LightMessage = "The filename specified has an invalid extension.";
                        Status = LightStatus.Error;
                    }
                }
                else
                {
                    if (MessageBox.Show("File " + tbFilename.Text + " already exists.\nDo you want to replace it?", "Save As", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Status = LightStatus.Ok;
                        this.LightMessage = "The filename will be overwritten.";
                    }
                    else
                    {
                        Status = LightStatus.Error;
                        this.LightMessage = "A filename already exists by that name.";
                    }
                }
            }
            else
            {
                if (m_FileAccess == FileAccessType.Open)
                {
                    Status = LightStatus.Error;
                    this.LightMessage = "The specified filename was not found.";
                }
                else
                {
                    string ext = System.IO.Path.GetExtension(tbFilename.Text);
                    if (IsValidExtension(ext))
                    {
                        this.LightMessage = "The filename specified is valid.";
                        Status = LightStatus.Ok;

                    }
                    else
                    {
                        this.LightMessage = "The filename specified has an invalid extension for saving.";
                        Status = LightStatus.Error;
                    }
                }
            }
        }

       

        #endregion 

        #region Private Functions
        private bool IsValidExtension(string Extension)
        {
            string[] GridOpenExtensions = { ".adf", ".bgd", ".asc", ".tif", ".ddf", ".arc", ".aux", ".pix", ".dhm", ".dt0", ".dt1", ".bil" };
            string[] GridSaveExtensions = { ".bgd", ".asc", ".tif", ".ddf", ".aux", ".pix", ".dhm", ".dt0", ".dt1", ".bil" };
            string[] ImageOpenExtensions = { "hdr.adf", ".asc", ".bt", ".bil", ".bmp", ".ecw", ".img", ".gif", ".map", ".jp2", ".jpg", ".sid", ".pgm", ".pnm", ".png", ".ppm", ".tif" };
            string[] ImageSaveExtensions = { ".asc", ".bt", ".bil", ".bmp", ".ecw", ".img", ".gif", ".map", ".jp2", ".jpg", ".sid", ".pgm", ".pnm", ".png", ".ppm", ".tif" };
            string[] ValidExtensions = null;
           
            if(m_FileType == FileTypes.All)return true; // No validation for this extension
            if (m_FileType == FileTypes.Shapefile)
            {
                if (Extension == ".shp")
                {
                    return true;
                }
                else
                {

                    return false;
                }
            }
            if (m_FileType == FileTypes.Grid)
            {
                if (m_FileAccess == FileAccessType.Open)
                {
                    ValidExtensions = GridOpenExtensions;
                    
                }
                else
                {
                    ValidExtensions = GridSaveExtensions;

                }
            }
            if (m_FileType == FileTypes.Image)
            {
                if (m_FileAccess == FileAccessType.Open)
                {
                    ValidExtensions = ImageOpenExtensions;

                }
                else
                {
                    ValidExtensions = ImageSaveExtensions;

                }
            }

            int numElements = ValidExtensions.GetUpperBound(0);
            for (int I = 0; I < numElements; I++)
            {
                if (ValidExtensions[I] == Extension) return true;
            }
            return false;
        }

        #endregion

      

        void groupBox1_Click(object sender, System.EventArgs e)
        {
            OnClick(e);
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        // Moves selected files upwards to an earlier position
        private void cmdUp_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count < 1) return;
            if (dataGridView1.SelectedRows[0].Index == 0) return;
            DataRow dr = tblFiles.NewRow();
            int I = dataGridView1.SelectedRows[0].Index-1;
            int J = dataGridView1.SelectedRows[I + dataGridView1.SelectedRows.Count].Index - 1;
            if (J - I > dataGridView1.SelectedRows.Count)
            {
                // Discontinuous selection requires individual handling
                for (int K = 0; K < dataGridView1.SelectedRows.Count; K++)
                {
                    J = dataGridView1.SelectedRows[K].Index;
                    dr = tblFiles.Rows[J];
                    tblFiles.Rows.RemoveAt(J);
                    tblFiles.Rows.InsertAt(dr, J - 1);
                }
            }
            else
            {
                dr = tblFiles.Rows[I];
                tblFiles.Rows.Remove(dr);
                tblFiles.Rows.InsertAt(dr, J);
            }
        }

        // Moves the selected files down in the roster
        private void cmdDown_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count < 1) return;
            if (dataGridView1.SelectedRows[dataGridView1.SelectedRows.Count-1].Index == tblFiles.Rows.Count-1) return;
            DataRow dr = tblFiles.NewRow();
            int I = dataGridView1.SelectedRows[0].Index - 1;
            int J = dataGridView1.SelectedRows[I + dataGridView1.SelectedRows.Count].Index - 1;
            if (J - I > dataGridView1.SelectedRows.Count)
            {
                // Discontinuous selection requires individual handling
                for (int K = 0; K < dataGridView1.SelectedRows.Count; K++)
                {
                    //dr = dataGridView1.SelectedRows[K];
                    //tblFiles.Rows.Remove(dr);
                    //tblFiles.Rows.InsertAt(dr, J + 1);
                }
            }
            else
            {
                //dr = tblFiles.Rows[J];
                //tblFiles.Rows.Remove(dr);
                //tblFiles.Rows.InsertAt(I);
            }
        }

        



    }
}
