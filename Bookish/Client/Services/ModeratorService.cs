using Blazored.LocalStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookish.Client.Services
{
    public class ModeratorService
    {
        private bool _moderator { get; set; }
        public bool IsModerator { 
            get { 
                return _moderator; 
            } 
            set {
                _moderator = value;
            } 
        }
    }
}
