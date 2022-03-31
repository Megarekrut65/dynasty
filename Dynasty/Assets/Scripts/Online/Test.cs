using UnityEngine;
using Firebase.Database;

public class Test : MonoBehaviour {
	private void Start() {
		// Get the root reference location of the database.
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
		reference.Child("Hi").SetValueAsync("hello, world");
	}
}