// third-party
import { FormattedMessage } from 'react-intl';

// assets
import PieChartOutlined from '@ant-design/icons/PieChartOutlined';
import EnvironmentOutlined from '@ant-design/icons/EnvironmentOutlined';
import { BookOutlined } from '@ant-design/icons';
import { TeamOutlined } from '@ant-design/icons';
import { SafetyCertificateOutlined } from '@ant-design/icons';
import { UserOutlined } from '@ant-design/icons';
import { ProfileOutlined } from '@ant-design/icons';
import { SolutionOutlined } from '@ant-design/icons';


// type
import { NavItemType } from 'types/menu';

// icons
const icons = { PieChartOutlined, EnvironmentOutlined, BookOutlined, TeamOutlined, SafetyCertificateOutlined, UserOutlined, ProfileOutlined, SolutionOutlined };

// ==============================|| MENU ITEMS - FORMS & TABLES ||============================== //

const backOffice: NavItemType = {
  id: 'group-back-office',
  title: <FormattedMessage id="Back Office" />,
  icon: icons.UserOutlined,
  type: 'group',
  children: [
    {
      id: 'group-back-office-collapse',
      title: <FormattedMessage id="Candidates" />,
      type: 'collapse',
      icon: icons.UserOutlined,
      children: [
        {
          id: 'group-back-office-candidates',
          title: <FormattedMessage id="Candidate Details" />,
          type: 'item',
          icon: icons.ProfileOutlined,
          url: '/backoffice/candidates'
        },
        {
          id: 'group-back-office-candidates',
          title: <FormattedMessage id="Career Assigment" />,
          type: 'item',
          icon: icons.SolutionOutlined,
          url: '/backoffice/careers'
        },
      ]
    },
    {
      id: 'group-back-office-collapse',
      title: <FormattedMessage id="Training Centres" />,
      type: 'item',
      icon: icons.TeamOutlined,
      url: '/backoffice/trainingcentres'
    },
    {
      id: 'group-back-office-collapse',
      title: <FormattedMessage id="Courses" />,
      type: 'item',
      icon: icons.BookOutlined,
      url: '/backoffice/courses'
    },
    {
      id: 'group-back-office-collapse',
      title: <FormattedMessage id="Qualifications" />,
      type: 'item',
      icon: icons.SafetyCertificateOutlined,
      url: '/backoffice/qualifications'
    },
  ]
};

export default backOffice;
