using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;

//ESTA CLASE ES COMO EL MAPPEO DEL DOCUMENTO A GUARDAR EN FIRESTORE

[FirestoreData]
public struct Jugador 
{
    [FirestoreProperty]
    public string uid { get; set; }

    [FirestoreProperty]
    public string capitulo { get; set; }

    [FirestoreProperty]
    public Dictionary<string, bool> listaHitos { get; set; }

    [FirestoreProperty]
    public int muertes {  get; set; }

    [FirestoreProperty]
    public Timestamp horaExpulsion { get; set; }

}
