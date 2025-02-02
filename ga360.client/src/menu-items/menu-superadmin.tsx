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
import { WechatOutlined, AppstoreAddOutlined, ContactsOutlined } from '@ant-design/icons';
import { AuditOutlined } from '@ant-design/icons';
// type
import { NavItemType } from 'types/menu';

// icons
const icons = { PieChartOutlined, EnvironmentOutlined, BookOutlined, TeamOutlined, SafetyCertificateOutlined, UserOutlined, ProfileOutlined, SolutionOutlined, UploadOutlined, WechatOutlined, AppstoreAddOutlined, ContactsOutlined,AuditOutlined };

// ==============================|| MENU ITEMS - FORMS & TABLES ||============================== //

const menuSuperAdmin: NavItemType = {
  id: 'group-menu-admin',
  title: <FormattedMessage id=" " />,
  icon: icons.UserOutlined,
  type: 'group',
  children: [
    {
      id: 'menu-admin-dashboard',
      title: <FormattedMessage id="Dashboard" />,
      type: 'item',
      icon: icons.AppstoreAddOutlined,
      url: '/dashboard/default'
    },
    {
      id: 'menu-admin-training-centres',
      title: <FormattedMessage id="Training Centres" />,
      type: 'item',
      icon: icons.ContactsOutlined,
      url: '/backoffice/trainingcentres'    
    },
    {
      id: 'menu-admin-leads',
      title: <FormattedMessage id="Leads" />,
      type: 'item',
      icon: icons.WechatOutlined,
      url: '/backoffice/leads'
    },
    {
      id: 'menu-admin-learners',
      title: <FormattedMessage id="Learners" />,
      type: 'item',
      icon: icons.TeamOutlined,
      url: '/backoffice/candidates'
    },
    {
      id: 'menu-admin-qualifications',
      title: <FormattedMessage id="Qualifications" />,
      type: 'item',
      icon: icons.SafetyCertificateOutlined,
      url: '/backoffice/qualifications'
    },
    {
      id: 'menu-admin-courses',
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
    {
      id: 'menu-admin-audit-trail',
      title: <FormattedMessage id="Audit Trail" />,
      type: 'item',
      icon: icons.AuditOutlined,
      url: '/backoffice/audittrail'
    },
  ]
};
export default menuSuperAdmin;
