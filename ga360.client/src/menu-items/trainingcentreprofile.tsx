// third-party
import { FormattedMessage } from 'react-intl';

// assets
import { UserOutlined } from '@ant-design/icons';
import { FileTextOutlined } from '@ant-design/icons';


// type
import { NavItemType } from 'types/menu';

// icons
const icons = { UserOutlined, FileTextOutlined };

// ==============================|| MENU ITEMS - FORMS & TABLES ||============================== //

const trainingcentreprofile: NavItemType = {
  id: 'group-trainingcentreprofile',
  title: <FormattedMessage id="Training Centre Profile" />,
  icon: icons.UserOutlined,
  type: 'group',
  children: [
    {
      id: 'group-back-office-collapse-trainingcentreprofile',
      title: <FormattedMessage id="Management" />,
      type: 'collapse',
      icon: icons.UserOutlined,
      children: [
        {
          id: 'trainingcentreprofile-details',
          title: <FormattedMessage id="Dashboard" />,
          type: 'item',
          link: '/apps/profiles/trainingcentre/:tab',
          url: '/apps/profiles/trainingcentre/profile',
          breadcrumbs: false,
          icon: icons.FileTextOutlined
        }
      ]
    }
  ]
};

export default trainingcentreprofile;
