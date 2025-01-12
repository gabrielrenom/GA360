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
import { WechatOutlined, AppstoreAddOutlined } from '@ant-design/icons';
// type
import { NavItemType } from 'types/menu';

// icons
const icons = { PieChartOutlined, EnvironmentOutlined, BookOutlined, TeamOutlined, SafetyCertificateOutlined, UserOutlined, ProfileOutlined, SolutionOutlined, UploadOutlined, WechatOutlined, AppstoreAddOutlined };

// ==============================|| MENU ITEMS - FORMS & TABLES ||============================== //

const menuTrainingCentre: NavItemType = {
  id: 'group-menu-training-centre',
  title: <FormattedMessage id=" " />,
  icon: icons.UserOutlined,
  type: 'group',
  children: [
    {
      id: 'training-centre-dashboard',
      title: <FormattedMessage id="Dashboard" />,
      type: 'item',
      icon: icons.AppstoreAddOutlined,
      url: '/dashboard/default'
    },
    {
      id: 'training-centre-leads',
      title: <FormattedMessage id="Leads" />,
      type: 'item',
      icon: icons.WechatOutlined,
      url: '/backoffice/leads'
    },
    {
      id: 'training-centre-learners',
      title: <FormattedMessage id="Learners" />,
      type: 'item',
      icon: icons.TeamOutlined,
      url: '/backoffice/candidates'
    },
    {
      id: 'training-centre-qualifications',
      title: <FormattedMessage id="Qualifications" />,
      type: 'item',
      icon: icons.SafetyCertificateOutlined,
      url: '/backoffice/qualifications'
    },
    {
      id: 'training-centre-courses',
      title: <FormattedMessage id="Courses" />,
      type: 'item',
      icon: icons.ProfileOutlined,
      url: '/backoffice/courses'
    },
    {
      id: 'training-centre-batch-upload',
      title: <FormattedMessage id="Batch Upload" />,
      type: 'item',
      icon: icons.UploadOutlined,
      url: '/backoffice/candidatebatchuploader'
    },
  ]
};
export default menuTrainingCentre;
