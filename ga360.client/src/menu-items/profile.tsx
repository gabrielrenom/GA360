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

const profile: NavItemType = {
  id: 'group-profile',
  title: <FormattedMessage id="Profile" />,
  icon: icons.UserOutlined,
  type: 'group',
  children: [
    {
      id: 'group-back-office-collapse',
      title: <FormattedMessage id="Bio" />,
      type: 'collapse',
      icon: icons.UserOutlined,
      children: [
        {
          id: 'bio-details',
          title: <FormattedMessage id="candidate-profile" />,
          type: 'item',
          link: '/apps/profiles/candidate/:tab',
          url: '/apps/profiles/candidate/profile',
          breadcrumbs: false,
          icon: icons.FileTextOutlined
        }
      ]
    }
  ]
};

export default profile;
