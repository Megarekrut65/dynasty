public static class Translator {
	public static string Translate(string text) {
		string[] keys = text.Split(' ');
		string res = "";
		for (int i = 0; i < keys.Length; i++) {
			res += TranslateWord(keys[i]);
			if (i != keys.Length - 1) res += " ";
		}
		return res;
	}
	private static string TranslateWord(string key) {
		string word = LocalizationManager.Instance.GetWord(key);
		if (word == LocalizationMap.NOT_FOUND) word = key;
		return word;
	}
}