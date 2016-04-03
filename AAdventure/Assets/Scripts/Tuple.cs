using UnityEngine;
using System.Collections;

public class Tuple {
	public int x;
	public int y;
	public Tuple(int x, int y) {
		this.x = x;
		this.y = y;
	}

	public override bool Equals(System.Object obj) {
		if (obj == null) {
			return false;
		}

		Tuple t = (Tuple)obj;
		if((System.Object) t == null) {
			return false;
		}

		return (x == t.x) && (y == t.y);
	}

	public bool Equals(Tuple t) {
		if ((object)t == null) {
			return false;
		}
		return (x == t.x) && (y == t.y);
	}

	public override int GetHashCode() {
		return x ^ y;
	}
}
