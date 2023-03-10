using System.Linq;
using System.Collections.Generic;

namespace Oxide.Plugins
{
    [Info("You've Got Mail", "KajWithAJ", "0.0.3")]
    [Description("Notifies online players when they receive mail in their mailbox.")]
    class YouveGotMail : RustPlugin
    {
        private const string MailboxPermission = "youvegotmail.message";

        private void Init()
        {
            permission.RegisterPermission(MailboxPermission, this);
        }

        protected override void LoadDefaultMessages()
        {
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["ReceivedMail"] = "<color=#FFFFFF>You've received mail from <color=#FFFF00>{0}</color> in your mailbox at <color=#FFFF00>{1}</color>.</color>"
            }, this);
        }

        void OnItemSubmit(Item item, Mailbox mailbox, BasePlayer player)
        {
            if (mailbox.ShortPrefabName == "mailbox.deployed") {
                ulong ownerID = mailbox.OwnerID;

                BasePlayer owner = BasePlayer.FindByID(ownerID);
                if (owner != null) {
                    if (permission.UserHasPermission(owner.UserIDString, MailboxPermission)) {
                        string coordinates = PhoneController.PositionToGridCoord(mailbox.transform.position);
                        string message = lang.GetMessage("ReceivedMail", this, owner.UserIDString);
                        Player.Message(owner, string.Format(message, player.displayName, coordinates));
                    }
                }
            }
        }
    }
}
