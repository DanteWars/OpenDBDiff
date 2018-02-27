using System;
using OpenDBDiff.Schema.Model;

namespace OpenDBDiff.Schema.SQLServer.Generates.Model
{
    public class Trigger : Code
    {
        public Trigger(ISchemaBase parent)
            : base(parent, ObjectType.Trigger, ScriptAction.AddTrigger, ScriptAction.DropTrigger)
        {
            this.Parent = parent;
        }

        /// <summary>
        /// Clona el objeto en una nueva instancia.
        /// </summary>
        public override ISchemaBase Clone(ISchemaBase parent)
        {
            Trigger trigger = new Trigger(parent);
            trigger.Text = this.Text;
            trigger.Status = this.Status;
            trigger.Name = this.Name;
            trigger.IsDisabled = this.IsDisabled;
            trigger.InsteadOf = this.InsteadOf;
            trigger.NotForReplication = this.NotForReplication;
            trigger.Owner = this.Owner;
            trigger.Id = this.Id;
            trigger.IsDDLTrigger = this.IsDDLTrigger;
            trigger.Guid = this.Guid;
            return trigger;
        }

        public Boolean IsDDLTrigger { get; set; }

        public Boolean InsteadOf { get; set; }

        public Boolean IsDisabled { get; set; }

        public Boolean NotForReplication { get; set; }

        public override Boolean IsCodeType
        {
            get { return true; }
        }

        public override string ToSqlDrop()
        {
            if (!IsDDLTrigger)
                return "DROP TRIGGER " + FullName + "\r\nGO\r\n";
            else
                return "DROP TRIGGER " + FullName + " ON DATABASE\r\nGO\r\n";
        }

        public string ToSQLEnabledDisabled()
        {
            if (!IsDDLTrigger)
            {
                if (IsDisabled)
                    return "DISABLE TRIGGER [" + Name + "] ON " + Parent.FullName + "\r\nGO\r\n";
                else
                    return "ENABLE TRIGGER [" + Name + "] ON " + Parent.FullName + "\r\nGO\r\n";
            }
            else
            {
                if (IsDisabled)
                    return "DISABLE TRIGGER [" + Name + "]\r\nGO\r\n";
                else
                    return "ENABLE TRIGGER [" + Name + "]\r\nGO\r\n";
            }
        }

        public override SQLScriptList ToSqlDiff(System.Collections.Generic.ICollection<ISchemaBase> schemas)
        {
            SQLScriptList list = new SQLScriptList();
            if (this.Status == ObjectStatus.Drop)
                list.Add(Drop());
            if (this.Status == ObjectStatus.Create)
                list.Add(Create());
            if (this.HasState(ObjectStatus.Alter))
                list.AddRange(Rebuild());
            if (this.HasState(ObjectStatus.Disabled))
                list.Add(this.ToSQLEnabledDisabled(), 0, ScriptAction.EnabledTrigger);
            return list;
        }

        public override bool Compare(ICode obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if (!this.ToSql().Equals(obj.ToSql())) return false;
            if (this.InsteadOf != ((Trigger)obj).InsteadOf) return false;
            if (this.IsDisabled != ((Trigger)obj).IsDisabled) return false;
            if (this.NotForReplication != ((Trigger)obj).NotForReplication) return false;
            return true;
        }
    }
}
