export interface ReviewDTO {
  id: string;
  createdAt?: string;
  updatedAt?: string;
  rating?: string;
  comment: string;
  productName: string;
  customerName?: string;

  sentimentr?: string;
  fake?: boolean;
  sentimentScore?: string;
  fakeReason?: string;
  adminAutoReply?: string;
  analyzedAt?: string;
}
