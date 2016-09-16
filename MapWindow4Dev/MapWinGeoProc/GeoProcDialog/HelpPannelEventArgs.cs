using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
namespace MapWinGeoProc.Dialogs
{
     /// <summary>
        /// Contain a package of information for changing the help pannel to reflect
        /// information appropriate for the element responding to a help button click.
        /// </summary>
        public class HelpPannelEventArgs : System.EventArgs
        {
            string m_HelpTitle;
            string m_HelpText;
            Image m_HelpImage;
            /// <summary>
            /// Creates a new instance of the HelpPannelEventArgs class
            /// </summary>
            /// <param name="HelpTitle">A New help title for the pannel</param>
            /// <param name="HelpText">A paragraph of explaining text</param>
            /// <param name="HelpImage">An image for the pannel</param>
            public HelpPannelEventArgs(string HelpTitle, string HelpText, Image HelpImage)
            {
                m_HelpTitle = HelpTitle;
                m_HelpText = HelpText;
                m_HelpImage = HelpImage;
            }
            /// <summary>
            /// The string caption associated with the element
            /// </summary>
            public string Title
            {
                get
                {
                    return m_HelpTitle;
                }
            }
            /// <summary>
            /// The string text giving help for the element.
            /// </summary>
            public string Text
            {
                get
                {
                    return m_HelpText;
                }
            }
            /// <summary>
            /// The System.Drawing.Image providing a picture for the help panel.
            /// </summary>
            public Image Image
            {
                get
                {
                    return m_HelpImage;
                }
            }
        }


}
