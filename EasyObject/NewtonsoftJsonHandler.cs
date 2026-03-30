namespace Global;
internal class NewtonsoftJsonHandler : IParseJson {
    public object ParseJson(string json) {
        return NewtonsoftJsonUtil.DeserializeFromJson(json);
    }
    public object[] ParseJsonSequence(string jsonSequenceString) {
        return [this.ParseJson(jsonSequenceString)];
    }
}