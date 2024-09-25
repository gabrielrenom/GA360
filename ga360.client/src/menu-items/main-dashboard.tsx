// third-party
import { FormattedMessage } from 'react-intl';

// assets
import DashboardOutlined from '@ant-design/icons/DashboardOutlined';
import GoldOutlined from '@ant-design/icons/GoldOutlined';
import LoadingOutlined from '@ant-design/icons/LoadingOutlined';


// type
import { NavItemType } from 'types/menu';

// icons
const icons = { dashboard: DashboardOutlined, components: GoldOutlined, loading: LoadingOutlined };

// ==============================|| MENU ITEMS - FORMS & TABLES ||============================== //

const mainDashboard: NavItemType = {
  id: 'group-dashboards',
  title: <FormattedMessage id="Dashboards" />,
  icon: icons.components,
  type: 'group',
  children: [
    {
      id: 'group-dashboards-collapse',
      title: <FormattedMessage id="Back Office Dashboards" />,
      type: 'collapse',
      icon: icons.dashboard,
      children: [
        {
          id: 'group-dashboards-default',
          title: <FormattedMessage id="Default" />,
          type: 'item',
          url: '/dashboard/default'
        },
      ]
    },
  ]
};

export default mainDashboard;
