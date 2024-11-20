using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FirestoreData]
public struct Estadisticas
{
    [FirestoreProperty]
    public int expulsados { get; set; }

    [FirestoreProperty]
    public int floresAsesinadas { get; set; }

    [FirestoreProperty]
    public int floresSalvadas { get; set; }

    [FirestoreProperty]
    public int insectosAsesinados { get; set; }

    [FirestoreProperty]
    public int insectosSalvados { get; set; }

    [FirestoreProperty]
    public int muertesTotales { get; set; }

    [FirestoreProperty]
    public int rojo { get; set; }

    [FirestoreProperty]
    public int verde { get; set; }

    [FirestoreProperty]
    public int perro { get; set; }

    [FirestoreProperty]
    public int gato { get; set; }

    [FirestoreProperty]
    public int ducha { get; set; }

    [FirestoreProperty]
    public int cama { get; set; }

    [FirestoreProperty]
    public int wifi { get; set; }

    [FirestoreProperty]
    public int corazon { get; set; }

    [FirestoreProperty]
    public int verdunchIzq { get; set; }

    [FirestoreProperty]
    public int verdunchDrch { get; set; }

    [FirestoreProperty]
    public int alquiler { get; set; }

    [FirestoreProperty]
    public int finalTrueBueno { get; set; }

    [FirestoreProperty]
    public int finalBueno { get; set; }

    [FirestoreProperty]
    public int finalMalo { get; set; }

    [FirestoreProperty]
    public int finalTrueMalo { get; set; }

    [FirestoreProperty]
    public int finalAlternativo { get; set; }

    [FirestoreProperty]
    public int pensamientosAplastados { get; set; }

}
