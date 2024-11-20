
using Firebase.Firestore;

[FirestoreData]
public struct Dialogos
{
    [FirestoreProperty]
    public string[] dialogos { get; set; }
}
