using System;
using System.Collections.Generic;
using System.Text;

using UniqueStudio.ComContent.Model;

namespace UniqueStudio.ComContent.BLL
{
    public class PostEventArgs
    {
        private PostInfo post;

        public PostEventArgs()
        {
        }

        public PostEventArgs(PostInfo post)
        {
            this.post = post;
        }

        public PostInfo Post
        {
            get { return post; }
            set { post = value; }
        }
    }
}
