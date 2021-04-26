using System;
using System.Data;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

[Serializable]
[SqlUserDefinedAggregate(
    Format.UserDefined, //use clr serialization to serialize the intermediate result
    IsInvariantToNulls = true, //optimizer property
    IsInvariantToDuplicates = false, //optimizer property
    IsInvariantToOrder = false, //optimizer property
    MaxByteSize = 8000) //maximum size in bytes of persisted value
]
public class StringJoin : IBinarySerialize
{
    private StringBuilder intermediateResult;
    public void Init()
    {
        this.intermediateResult = new StringBuilder();
    }

    public void Accumulate(SqlString Value)
    {
        if (!Value.IsNull)
            if (Value.ToString().Trim().Length > 0)
                this.intermediateResult.Append(Value.Value).Append(", ");    
    }

    public void Merge(StringJoin Group)
    {
        this.intermediateResult.Append(Group.intermediateResult);
    }

    public SqlString Terminate()
    {
        string output = string.Empty;
        if (this.intermediateResult != null && this.intermediateResult.Length > 2)
           output = this.intermediateResult.ToString(0, this.intermediateResult.Length - 2);

        return new SqlString(output);
    }
    public void Read(BinaryReader r)
    {
        intermediateResult = new StringBuilder(r.ReadString());
    }

    public void Write(BinaryWriter w)
    {
        w.Write(this.intermediateResult.ToString());
    }

}
