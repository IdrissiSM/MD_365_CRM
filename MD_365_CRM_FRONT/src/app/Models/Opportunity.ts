import { ProductOpportunity } from './ProductOpportunity';

export interface Opportunity {
    opportunityId?: string;
    name: string;
    emailAddress: string;
    estimatedValue: number;
    actualValue: number;
    estimatedCloseDate: Date;
    actualCloseDate: Date;
    closeProbability: number;
    statusCode: number;
    stateCode: number;
    opportunityRatingCode: number;
    completeInternalReview: boolean;
    stepName: string;
    customerNeed: string;
    currentSituation: string;
    proposedSolution: string;
    description: string;
    createdOn: Date;
    modifiedOn: Date;
    _parentContactId_value: string;
    _parentAccountid_value: string;
    product_opportunities: ProductOpportunity[];
}
