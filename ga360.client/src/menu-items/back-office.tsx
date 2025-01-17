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
import { UploadOutlined } from '@ant-design/icons'; // Import the UploadOutlined icon

// type
import { NavItemType } from 'types/menu';

// icons
const icons = { PieChartOutlined, EnvironmentOutlined, BookOutlined, TeamOutlined, SafetyCertificateOutlined, UserOutlined, ProfileOutlined, SolutionOutlined, UploadOutlined };

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
          id: 'group-back-office-candidates-candidate',
          title: <FormattedMessage id="Learner Details" />,
          type: 'item',
          icon: icons.ProfileOutlined,
          url: '/backoffice/candidates'
        },
        {
          id: 'group-back-office-candidates-career',
          title: <FormattedMessage id="Career Assigment" />,
          type: 'item',
          icon: icons.SolutionOutlined,
          url: '/backoffice/careers'
        },
        {
          id: 'group-back-office-candidates-batch-upload',
          title: <FormattedMessage id="Candidate Batch Upload" />,
          type: 'item',
          icon: icons.UploadOutlined,
          url: '/backoffice/candidatebatchuploader'
        },
      ]
    },
    {
      id: 'group-back-office-collapse-training-centres',
      title: <FormattedMessage id="Training Centres" />,
      type: 'item',
      icon: icons.TeamOutlined,
      url: '/backoffice/trainingcentres'
    },
    {
      id: 'group-back-office-collapse-courses',
      title: <FormattedMessage id="Courses" />,
      type: 'item',
      icon: icons.BookOutlined,
      url: '/backoffice/courses'
    },
    {
      id: 'group-back-office-collapse-qualifications',
      title: <FormattedMessage id="Qualifications" />,
      type: 'item',
      icon: icons.SafetyCertificateOutlined,
      url: '/backoffice/qualifications'
    },
  ]
};

export default backOffice;
