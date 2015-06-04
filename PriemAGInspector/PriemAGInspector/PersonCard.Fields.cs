using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriemAGInspector
{
    public partial class PersonCard
    {
        public string Surname
        {
            get
            {
                return tbSurname.Text;
            }
            set
            {
                tbSurname.Text = value;
            }
        }
        public string PersonName
        {
            get
            {
                return tbName.Text;
            }
            set
            {
                tbName.Text = value;
            }
        }
        public string SecondName
        {
            get
            {
                return tbSecondName.Text;
            }
            set
            {
                tbSecondName.Text = value;
            }
        }
        public bool Sex
        {
            get
            {
                return rbMale.IsChecked;
            }
            set
            {
                if (value)
                    rbMale.IsChecked = true;
                else
                    rbFemale.IsChecked = true;
            }
        }
        public DateTime BirthDate
        {
            get
            {
                return dtpBirthDate.Value;
            }
            set
            {
                dtpBirthDate.Value = value;
            }
        }
        public string BirthPlace
        {
            get
            {
                return tbBirthPlace.Text;
            }
            set
            {
                tbBirthPlace.Text = value;
            }
        }
        public int Nationality
        {
            get
            {
                return (int)ddlNationality.SelectedValue;
            }
            set
            {
                ddlNationality.SelectedValue = value;
            }
        }
        public int PassportType
        {
            get
            {
                return (int)ddlPassportType.SelectedValue;
            }
            set
            {
                ddlPassportType.SelectedValue = value;
            }
        }
        public string PassportSeries
        {
            get
            {
                return tbPassportSeries.Text;
            }
            set
            {
                tbPassportSeries.Text = value;
            }
        }
        public string PassportNumber
        {
            get
            {
                return tbPassportNumber.Text;
            }
            set
            {
                tbPassportNumber.Text = value;
            }
        }
        public DateTime PassportDate
        {
            get
            {
                return dtpPassportDate.Value;
            }
            set
            {
                dtpPassportDate.Value = value;
            }
        }
        public string PassportAuthor
        {
            get
            {
                return tbPassportAuthor.Text;
            }
            set
            {
                tbPassportAuthor.Text = value;
            }
        }
        public string PassportCode
        {
            get
            {
                return tbPassportCode.Text;
            }
            set
            {
                tbPassportCode.Text = value;
            }
        }

        public string MainPhone
        {
            get
            {
                return tbMainPhone.Text;
            }
            set
            {
                tbMainPhone.Text = value;
            }
        }
        public string AddPhone
        {
            get
            {
                return tbAddPhone.Text;
            }
            set
            {
                tbAddPhone.Text = value;
            }
        }
        public string Email
        {
            get
            {
                return tbEmail.Text;
            }
            set
            {
                tbEmail.Text = value;
            }
        }
        public int? Country
        {
            get
            {
                return (int)ddlCountry.SelectedValue;
            }
            set
            {
                ddlCountry.SelectedValue = value;
            }
        }
        public int? RegionId
        {
            get
            {
                return (int)ddlRegion.SelectedValue;
            }
            set
            {
                ddlRegion.SelectedValue = value;
            }
        }
        public string City
        {
            get
            {
                return tbCity.Text;
            }
            set
            {
                tbCity.Text = value;
            }
        }
        public string PostCode
        {
            get
            {
                return tbPostCode.Text;
            }
            set
            {
                tbPostCode.Text = value;
            }
        }
        public string Street
        {
            get
            {
                return tbStreet.Text;
            }
            set
            {
                tbStreet.Text = value;
            }
        }
        public string House
        {
            get
            {
                return tbHouse.Text;
            }
            set
            {
                tbHouse.Text = value;
            }
        }
        public string Korpus
        {
            get
            {
                return tbKorpus.Text;
            }
            set
            {
                tbKorpus.Text = value;
            }
        }
        public string Flat
        {
            get
            {
                return tbFlat.Text;
            }
            set
            {
                tbFlat.Text = value;
            }
        }
        public string PostIndexReal
        {
            get
            {
                return tbPostCodeReal.Text;
            }
            set
            {
                tbPostCodeReal.Text = value;
            }

        }
        public string CityReal
        {
            get
            {
                return tbCityReal.Text;
            }
            set
            {
                tbCityReal.Text = value;
            }
        }
        public string StreetReal
        {
            get
            {
                return tbStreetReal.Text;
            }
            set
            {
                tbStreetReal.Text = value;
            }
        }
        public string HouseReal
        {
            get
            {
                return tbHouseReal.Text;
            }
            set
            {
                tbHouseReal.Text = value;
            }
        }
        public string KorpusReal
        {
            get
            {
                return tbKorpusReal.Text;
            }
            set
            {
                tbKorpusReal.Text = value;
            }
        }
        public string FlatReal
        {
            get
            {
                return tbFlatReal.Text;
            }
            set
            {
                tbFlatReal.Text = value;
            }
        }

        public int? RegionEducId
        {
            get
            {
                if (ddlRegionEduc.SelectedValue == null)
                    return 0;
                return (int)ddlRegionEduc.SelectedValue;
            }
            set
            {
                ddlRegionEduc.SelectedValue = value;
            }
        }
        public int? CountryEduc
        {
            get
            {
                return (int)ddlCountryEduc.SelectedValue;
            }
            set
            {
                ddlCountryEduc.SelectedValue = value;
            }
        }
        public string SchoolName
        {
            get
            {
                return tbSchoolName.Text;
            }
            set
            {
                tbSchoolName.Text = value;
            }
        }
        public string SchoolNum
        {
            get
            {
                return tbSchoolNum.Text;
            }
            set
            {
                tbSchoolNum.Text = value;
            }
        }
        public string SchoolCity
        {
            get
            {
                return tbSchoolCity.Text;
            }
            set
            {
                tbSchoolCity.Text = value;
            }
        }

        public int SchoolExitClass
        {
            get
            {
                return (int)ddlSchoolExitClass.SelectedValue;
            }
            set
            {
                ddlSchoolExitClass.SelectedValue = value;
            }
        }
        public string DocumentSeries
        {
            get
            {
                return tbDocumentSeries.Text;
            }
            set
            {
                tbDocumentSeries.Text = value;
            }
        }
        public string DocumentNumber
        {
            get
            {
                return tbDocumentNumber.Text;
            }
            set
            {
                tbDocumentNumber.Text = value;
            }
        }
    }
}
