export interface Project {
  id: string;
  name: string;
  key: string;
  description: string;
  isArchived: boolean;
  createdAt: string;
}

export interface CreateProjectRequest {
  name: string;
  key: string;
  description: string;
}
