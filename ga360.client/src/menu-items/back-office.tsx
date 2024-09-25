// third-party
import { FormattedMessage } from 'react-intl';

// assets
import PieChartOutlined from '@ant-design/icons/PieChartOutlined';
import EnvironmentOutlined from '@ant-design/icons/EnvironmentOutlined';

// type
import { NavItemType } from 'types/menu';

// icons
const icons = { PieChartOutlined, EnvironmentOutlined };

// ==============================|| MENU ITEMS - FORMS & TABLES ||============================== //

const backOffice: NavItemType = {
  id: 'group-back-office',
  title: <FormattedMessage id="Back Office" />,
  icon: icons.PieChartOutlined,
  type: 'group',
  children: [
    {
      id: 'group-back-office-collapse',
      title: <FormattedMessage id="Candidates" />,
      type: 'collapse',
      icon: icons.PieChartOutlined,
      children: [
        {
          id: 'group-back-office-candidates',
          title: <FormattedMessage id="Candidate Details" />,
          type: 'item',
          url: '/backoffice/candidates'
        },
      ]
    },
  ]
};

export default backOffice;
