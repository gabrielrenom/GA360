﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA360.DAL.Entities.Enums;

public class StatusEnum
{
    public enum ClientType
    {
        Prospect = 0,
        Reseller = 1,
        Partner = 2,
        Vendor = 3,
        Other = 4
    }
    public enum PotentialType
    {
        Prospect = 0,
        ResourceDeficit = 1,
        Standard = 2,
        Minor = 3,
        Other = 4
    }
    public enum ProjectType
    {
        ODC = 0,
        FixedPrice = 1,
        TNM = 2,
        Internal = 3
    }
    public enum ProjectStatus
    {
        Pending = 0,
        OnGoing = 1,
        Finish = 2
    }
    public enum ContractType
    {
        ODC = 0,
        FixedPrice = 1,
        TNM = 2,
        Internal = 3
    }
    public enum ContractStatus
    {
        Pending = 0,
        OnGoing = 1,
        Finish = 2
    }
    public enum InvoiceStatus
    {
        Pending = 0,
        Chasing = 1,
        Fail = 2,
        Paid = 3,
    }
    public enum InvoiceFileType
    {
        PDF,
        Excel,
        Word,
        Image
    }
    public enum CurrencyType
    {
        USD = 0,
        VND = 1,
        EUR = 2,
        JPY = 3
    }

    public enum ClientStatus
    {
        New = 0,
        RegularContact = 1,
        InactiveContact = 2,
        Verified = 3,
        Pending = 4,
        Rejected = 5
    }
    public enum LeadStatus
    {
        New,
        Contacted,
        Qualified,
        ProposalSent,
        Negotiation,
        Won,
        Lost
    }
    public enum ProjectTypeList
    {
        ODC,
        FixedPrice,
        TNM,
        Internal
    }

    public enum EntityDefault
    {
        Invoice,
        Client,
        Project,
        Contract,
        Deal,
        Assignment,
        Contact
    }

    public enum Status
    {
        RequestCome = 0,
        ProcessingRequest = 1,
        CustomerInProgress = 2,
        CustomerWin = 3,
        CustomerFail = 4,
        DealLost = 5
    }

    public enum AssignmentStatus
    {
        Todo = 0,
        InProgress = 1,
        Done = 2
    }

    public enum TeamMember
    {
        Leader = 1,
        ViceLeader = 2,
        Member = 3
    }

    public enum Priority
    {
        Block,
        Trival,
        Minor,
        Major,
        Critical,
    }
    public enum CampaignContactStatus
    {
        Unsent,
        Sending,
        Sent,
        EmailNotExist
    }
}
