Namespace Servers

    Public NotInheritable Class ServerMessages

#Region "Enums"

#End Region

#Region "Fields and Constants"

        Public Const GUI_PLAYERLIST_TITLE As String = "Players ({0} / {1})"
        Public Const GUI_PLAYERLIST_ENTRY As String = "{0} ({1}){2}"
        Public Const GUI_PLAYERMENU_UNMUTE As String = "Unmute"
        Public Const GUI_PLAYERMENU_MUTE As String = "Mute"
        Public Const GUI_PLAYERMENU_REMOVE_WHITELIST As String = "Remove from Whitelist"
        Public Const GUI_PLAYERMENU_ADD_WHITELIST As String = "Add to Whitelist"
        Public Const GUI_PLAYERMENU_REMOVE_BLACKLIST As String = "Remove from Blacklist"
        Public Const GUI_PLAYERMENU_ADD_BLACKLIST As String = "Add to Blacklist"
        Public Const GUI_PLAYERMENU_REMOVE_OPERATOR As String = "Remove Operator permissions"
        Public Const GUI_PLAYERMENU_ADD_OPERATOR As String = "Make Operator"

        Public Const SERVER_START As String = "[INFO] Starting Pokémon3D Server with protocol version ""{0}""."
        Public Const SERVER_CLOSE As String = "[INFO] Closing host window ({0})"

        Public Const SERVER_OFFLINEWARNING1 As String = "[WARNING] Starting the server In offline mode. Players With hacked profiles are able To join the server."
        Public Const SERVER_OFFLINEWARNING2 As String = "[INFO] Offline Mode still prevents Online players To trade With Offline players."
        Public Const SERVER_STARTGAMEMODEINFO As String = "[INFO] Starting With GameMode: ""{0}"""

        Public Const SERVER_HOSTINFO As String = "[INFO] Hosting the server On {0}:{1}"
        Public Const SERVER_ERRORHOST As String = "[ERROR] Error starting the hosting process: {0}."
        Public Const SERVER_STOP As String = "[INFO] Stopping the server..."
        Public Const SERVER_RESTART As String = "[INFO] Restarting the server..."

        Public Const SERVER_WHITELIST_ON As String = "[INFO] Enabled the whitelist."
        Public Const SERVER_WHITELIST_OFF As String = "[INFO] Disabled the whitelist."
        Public Const SERVER_BLACKLIST_ON As String = "[INFO] Enabled the blacklist."
        Public Const SERVER_BLACKLIST_OFF As String = "[INFO] Disabled the blacklist."
        Public Const SERVER_OP_ON As String = "[INFO] Allowed operators."
        Public Const SERVER_OP_OFF As String = "[INFO] Disallowed operators."

        Public Const SERVER_LIST_REMOVE As String = "[INFO] Removed ""{0}"" from the list ""{1}""."
        Public Const SERVER_LIST_ADD As String = "[INFO] Added ""{0}"" to the list ""{1}""."
        Public Const SERVER_LIST_EXISTS As String = "[INFO] The player ""{0}"" is on the list ""{1}""."
        Public Const SERVER_LIST_NOT_EXISTS As String = "[INFO] The player ""{0}"" is not on the list ""{1}""."
        Public Const SERVER_LIST_NEW As String = "[INFO] Creating new player list ""{0}"" at {1}!"

        Public Const SERVER_PM_SENT As String = "[INFO] Sent a private message to the player ""{0}""."
        Public Const SERVER_PM_NOTARGET As String = "[INFO] Cannot find the specified playername."

        Public Const SERVER_INVALIDCOMMAND As String = "[INFO] ""{0}"" is not a valid command."

        Public Const SERVER_INVALIDPROTOCOL As String = "[INFO] A client with a different network protocol attempted to join or the data sent is invalid."
        Public Const SERVER_UNEXPECTEDCLIENTERROR As String = "[WARNING] Unexpected error while receiving client data: "
        Public Const SERVER_FULLSERVER As String = "[INFO] Player tried to connect, but the server is full."
        Public Const SERVER_WRONGGAMEMODE As String = "[INFO] Player with a GameMode ""{0}"" tried to join."
        Public Const SERVER_DUPLICATENAME As String = "[INFO] Player with the name ""{0}"" tried to join. This name already exists on the server!"
        Public Const SERVER_GAMEJOLTREQUIRED As String = "[INFO] Player {0} tried to join with an offline profile, OfflineMode is set to False."
        Public Const SERVER_WHITELIST As String = "[INFO] Player {0} tried to join but is not whitelisted."
        Public Const SERVER_BLACKLIST As String = "[INFO] Player {0} tried to join but is blacklisted."
        Public Const SERVER_NOOPWARNING As String = "[INFO] Player {0} tried to use a command without operator permissions!"
        Public Const SERVER_PLAYERKICKED As String = "[INFO] {0} got kicked from the server."

        Public Const SERVER_NEWSERVERMESSAGE As String = "[INFO] The server message changed to: {0}"

        Public Const PROPERTIES_NOFILEFOUND As String = "[WARNING] properties.dat does not exist!"
        Public Const PROPERTIES_LOADING As String = "[INFO] Loading properties."
        Public Const PROPERTIES_NEWFILE As String = "[INFO] Generating new properties file."

        Public Const CLIENT_FULLSERVER As String = "The server limit is reached."
        Public Const CLIENT_WRONGGAMEMODE As String = "The server requires the GameMode ""{0}""."
        Public Const CLIENT_DUPLICATENAME As String = "A player with the name ""{0}"" already exists on the server."
        Public Const CLIENT_GAMEJOLTREQUIRED As String = "Server requires a GameJolt profile to join."
        Public Const CLIENT_WHITELIST As String = "You have to be whitelisted to join this server."
        Public Const CLIENT_BLACKLIST As String = "You are banned from this server!"
        Public Const CLIENT_NOOPWARNING As String = "You don't have permissions to use commands on this server!"
        Public Const CLIENT_IDLEKICK As String = "Bad connection speed or inactivity."
        Public Const CLIENT_AFKKICK As String = "Inactivity."
        Public Const CLIENT_KICKED As String = "You got kicked from the server."
        Public Const CLIENT_SERVERCLOSED As String = "The server closed down."
        Public Const CLIENT_SERVERRESTART As String = "The server is restarting."
        Public Const CLIENT_NEWPLAYER As String = "{0} joined the game!"

        Public Const CLIENT_GAMESTATEMESSAGE As String = "The player {0} {1}"
        Public Const CLIENT_ISMUTED As String = "You are muted on this server!"
        Public Const CLIENT_NODESTINATIONPLAYER As String = "The player with the name ""{0}"" doesn't exist."

        Public Const PLAYERLIST_INACTIVE As String = "Inactive"
        Public Const PLAYERLIST_CHATTING As String = "Chatting"
        Public Const PLAYERLIST_BATTLING As String = "Battling"

#End Region

#Region "Properties"

#End Region

#Region "Delegates"

#End Region

#Region "Constructors"

        Private Sub New()
            '//Empty constructor to prevent instances.
        End Sub

#End Region

#Region "Methods"

#End Region

    End Class

End Namespace