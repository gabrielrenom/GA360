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

// ==============================|| MENU ITEMS ||============================== //

const menuItems: { items: NavItemType[] } = {  
  items: [backOffice,widget, applications, formsTables, chartsMap, samplePage, pages, other]


};

export default menuItems;
