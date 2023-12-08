
//0
/*
Kada se igrac uloguje dodaje se u listu aktivnih igraca
Na serveru u metodi OnConnectedAsync() dodati igraca u listu aktivnih igraca
*/

//1
/*
Igrac je kliknuo Join Lobby
Na serveru se poziva metoda "JoinLobby(int playerId) -> igrac se dodaje u lobby sa svojim id-em"
Na klijentu se poziva metoda "UpdateLobby(List<{playerUsername, playerAvatar}>})"
*/