// // project import
// import applications from './applications';
// import widget from './widget';
// import formsTables from './forms-tables';
// import samplePage from './sample-page';
// import chartsMap from './charts-map';
// import other from './other';
// import pages from './pages';

// // types
// import { NavItemType } from 'types/menu';
// import backOffice from './back-office';
// import mainDashboard from './main-dashboard';
// import profile from './profile';
// import development from './development';

// // ==============================|| MENU ITEMS ||============================== //

// const menuItems: { items: NavItemType[] } = {  
//   //items: [mainDashboard, backOffice,widget, applications, formsTables, chartsMap, samplePage, pages, other]
//   items: [mainDashboard,profile, backOffice, development ]

// };

// export default menuItems;

// project import
import applications from './applications';
import widget from './widget';
import formsTables from './forms-tables';
import samplePage from './sample-page';
import chartsMap from './charts-map';
import other from './other';
import pages from './pages';

// types
import { NavItemType } from 'types/menu';
import backOffice from './back-office';
import mainDashboard from './main-dashboard';
import profile from './profile';
import development from './development';
import trainingcentreprofile from './trainingcentreprofile';
// ==============================|| MENU ITEMS ||============================== //


const allMenuItems: { items: NavItemType[] } = {  
  items: [mainDashboard, profile, trainingcentreprofile, backOffice, development]
};

// Function to filter menu items based on user role
export const getMenuItemsByRole = (role: string): { items: NavItemType[] } => {
  let filteredItems = [];

    switch (role) {
      case "Training Centre":
        filteredItems = [
          'group-dashboards',
          'group-back-office',
          'group-trainingcentreprofile'
        ];
        break;
      case "Super Admin":
        filteredItems = allMenuItems.items.map(item => item.id)
        .filter(itemId => itemId !== 'group-trainingcentreprofile');;
        break;
      case "Candidate":
        filteredItems = ['group-profile'];
        break;
      case "Assessor":
        filteredItems = ['group-profile'];
        break;
      default:
        filteredItems = [];
    }

  const filteredMenuItems = allMenuItems.items.filter(item => {
    if (item.id === 'group-back-office' && role === 'Training Centre') {
      item.children = item.children?.filter(child => child.id !== 'group-back-office-collapse-training-centres');
      item.children = item.children?.filter(child => child.id !== 'group-back-office-collapse-courses');
      item.children = item.children?.filter(child => child.id !== 'group-back-office-collapse-qualifications');

    }
    return filteredItems.includes(item.id);
  });

  return { items: filteredMenuItems };
};


export default allMenuItems;

