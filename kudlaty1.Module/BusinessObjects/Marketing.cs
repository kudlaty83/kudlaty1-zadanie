using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using kudlaty1.Module.BusinessObjects;
using System;

namespace SimpleProjectManager.Module.BusinessObjects.Marketing
{
    [NavigationItem("Operacje")]
    public class Customer : BaseObject
    {
        public Customer(Session session) : base(session) { }
        string firstName;
        public string FirstName
        {
            get { return firstName; }
            set { SetPropertyValue(nameof(FirstName), ref firstName, value); }
        }
        string lastName;
        public string LastName
        {
            get { return lastName; }
            set { SetPropertyValue(nameof(LastName), ref lastName, value); }
        }
        string email;
        public string Email
        {
            get { return email; }
            set { SetPropertyValue(nameof(Email), ref email, value); }
        }

        Firma firma;
        public Firma Firma
        {
            get { return firma; }
            set { SetPropertyValue(nameof(Firma), ref firma, value); }
        }

        string occupation;
        public string Occupation
        {
            get { return occupation; }
            set { SetPropertyValue(nameof(Occupation), ref occupation, value); }
        }
        [Association("Customer-Testimonials")]
        public XPCollection<Testimonial> Testimonials
        {
            get { return GetCollection<Testimonial>(nameof(Testimonials)); }
        }
        
        byte[] photo;
        [ImageEditor(ListViewImageEditorCustomHeight = 75, DetailViewImageEditorFixedHeight = 150)]
        public byte[] Photo
        {
            get { return photo; }
            set { SetPropertyValue(nameof(Photo), ref photo, value); }
        }
    }
    [NavigationItem("Operacje")]
    public class Testimonial : BaseObject
    {
        public Testimonial(Session session) : base(session)
        {
            createdOn = DateTime.Now;
        }
        string quote;
        public string Quote
        {
            get { return quote; }
            set { SetPropertyValue(nameof(Quote), ref quote, value); }
        }
        string highlight;
        [Size(512)]
        public string Highlight
        {
            get { return highlight; }
            set { SetPropertyValue(nameof(Highlight), ref highlight, value); }
        }
        DateTime createdOn;
        [VisibleInListView(false)]
        public DateTime CreatedOn
        {
            get { return createdOn; }
            internal set { SetPropertyValue(nameof(CreatedOn), ref createdOn, value); }
        }
        string tags;
        public string Tags
        {
            get { return tags; }
            set { SetPropertyValue(nameof(Tags), ref tags, value); }
        }
        Customer customer;
        [Association("Customer-Testimonials")]
        public Customer Customer
        {
            get { return customer; }
            set { SetPropertyValue(nameof(Customer), ref customer, value); }
        }
    }
}