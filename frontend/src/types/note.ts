export type Note = {
    id: string;
    title: string;
    content?: string | null;
    createdAt: string;
    updatedAt: string;
};

export type NoteCreateRequest = {
    title: string;
    content?: string | null;
};

export type NoteUpdateRequest = {
    title: string;
    content?: string | null;
};


