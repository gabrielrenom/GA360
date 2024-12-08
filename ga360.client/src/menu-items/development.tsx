// third-party
import { FormattedMessage } from 'react-intl';

// assets
import { ApiOutlined } from '@ant-design/icons';
import { RocketOutlined } from '@ant-design/icons';
import { BookOutlined } from '@ant-design/icons';


// type
import { NavItemType } from 'types/menu';

// icons
const icons = {ApiOutlined, RocketOutlined,BookOutlined };

// ==============================|| MENU ITEMS - FORMS & TABLES ||============================== //


const development: NavItemType = {
  id: 'other',
  type: 'group',
  children: [
    {
      id: 'group-development',
      title: <FormattedMessage id="Development API" />,
      icon: icons.ApiOutlined,
      type: 'collapse',
      children: [
        {
          id: 'getting-started',
          title: <FormattedMessage id="Getting Started" />,
          type: 'item',
          icon: icons.RocketOutlined,
          url: '/development/gettingstarted'
        },
        {
          id: 'api-reference',
          title: <FormattedMessage id="Api Reference" />,
          type: 'item',
          icon: icons.BookOutlined,
          url: '/development/apireference'
        }
      ]
    }
  ]
};

export default development;
