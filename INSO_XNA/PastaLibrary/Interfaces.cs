using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PastaGameLibrary
{
	public interface IPComponent
	{
		bool Enabled { get; set; }
		IPActor Container { get; }
		void Initialise();

		void Attach(IPActor container);
	}
	public interface IPUpdatable
	{
		void Update();
	}
	public interface IPDrawable
	{
		void Draw();
	}
	public interface IPActor
	{
		int ID { get; }
		IPActor Parent { get; }
		List<IPActor> Children { get; }
		MyGame TheGame { get; }

		bool BindParent(IPActor parent);
		bool BindChild(IPActor child);
		void UnbindParent();
		void UnbindChild(IPActor child);
		void UnbindChildren();

		void AddComponent(IPComponent component);
		List<ComponentType> GetComponents<ComponentType>() where ComponentType : PComponent;
		ComponentType GetFirstComponent<ComponentType>() where ComponentType : PComponent;
	}
}
