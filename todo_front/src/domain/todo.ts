export interface Project {
  id: string;
  name: string | null;
  description: string | null;
}

export interface TaskItem {
  id: string;
  name: string | null;
  description: string | null;
  storyPoints: number | null;
  createdAt: string;
  deadline: string | null;
  project: Project;
}

export interface ProjectInput {
  name: string;
  description: string | null;
}

export interface TaskInput {
  name: string;
  description: string | null;
  storyPoints: number | null;
  deadline: string | null;
  projectId: string;
}
